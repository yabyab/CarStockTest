using System.Data.SQLite;
using Dapper;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class DealerRepository : IDealerRepository
    {
        private readonly IConfiguration _configuration;

        public DealerRepository(IConfiguration configuration){
            _configuration = configuration;
        }

        public async Task<Dealer> GetByIdAsync(int dealerId)
        {
            using(var connection = GetConnection()){
                var resDealer = await connection.QuerySingleOrDefaultAsync<Dealer>("SELECT * FROM Dealer WHERE dealerid = @id;", new {id = dealerId});
                return resDealer;
            }
        }

        public async Task<int> InsertDealerAsync(Dealer dealer)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    int? recCnt = await connection.QuerySingleOrDefaultAsync<int>(@"SELECT count(dealerid) FROM Dealer WHERE dealername = @dealername AND dealeremail = @dealeremail", new{ dealername = dealer.dealername, dealeremail = dealer.dealeremail}, transaction);
                    if(recCnt > 0){
                        await transaction.RollbackAsync();
                        throw new InvalidDataException("Dealer has been created.");
                    }
                    string sqlStr = @"
                    INSERT INTO Dealer (dealername, dealeremail) VALUES (@dealername, @dealeremail);
                    SELECT last_insert_rowid() AS dealerid FROM Dealer LIMIT 1;";
                    int resDealerId = await connection.QueryFirstAsync<int>(sqlStr, dealer, transaction);
                    if(resDealerId < 1){
                        await transaction.RollbackAsync();
                        throw new Exception("Dealer cannot be created.");
                    }
                    await transaction.CommitAsync();
                    return resDealerId;
                }
            }
        }

        public async Task UpdateDealerAsync(Dealer dealer)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    int? recCnt = await connection.QuerySingleOrDefaultAsync<int>(
                        @"SELECT count(dealerid) 
                        FROM Dealer 
                        WHERE dealername = @dealername AND dealeremail = @dealeremail AND dealerid != @dealerid ", dealer, transaction);
                    if(recCnt > 0){
                            await transaction.RollbackAsync();
                            throw new InvalidDataException("Dealer information gonna to update has already been used.");
                    }
                    recCnt = await connection.ExecuteAsync(
                        @"UPDATE Dealer 
                        SET dealername = @dealername, dealeremail = @dealeremail
                        WHERE dealerid = @dealerid", dealer, transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception("Dealer cannot be updated.");
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        private SQLiteConnection  GetConnection()
        {
            return new SQLiteConnection (_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}