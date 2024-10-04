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
                    int? recCnt = await connection.QuerySingleOrDefaultAsync<int>(@"SELECT count(dealerid) FROM Dealer WHERE dealername = @dealername AND dealeremail = @dealeremail",
                        new{
                            dealername = dealer.dealername,
                            dealeremail = dealer.dealeremail
                        },
                        transaction);
                    if(recCnt > 0){
                        await transaction.RollbackAsync();
                        throw new InvalidDataException("Dealer has been created.");
                    }
                    string sqlStr = @"
                    INSERT INTO Dealer (dealername, dealeremail, create_at, update_at) VALUES (@dealername, @dealeremail, @current_time, @current_time);
                    SELECT last_insert_rowid() AS dealerid FROM Dealer LIMIT 1;";
                    int resDealerId = await connection.QueryFirstAsync<int>(sqlStr,
                        new{
                            dealername = dealer.dealername,
                            dealeremail = dealer.dealeremail,
                            current_time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                        transaction);
                    if(resDealerId < 1){
                        await transaction.RollbackAsync();
                        throw new Exception("Dealer cannot be created.");
                    }
                    await transaction.CommitAsync();
                    return resDealerId;
                }
            }
        }

        public async Task<Dealer> UpdateDealerAsync(int dealerId, Dealer dealer)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    if(dealerId != dealer.dealerid){
                        throw new InvalidDataException("you are disallowed to modify other dealer's information.");
                    }
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
                        SET dealername = @dealername, dealeremail = @dealeremail, update_at = @updateTime 
                        WHERE dealerid = @dealerid", 
                        new {
                            dealername = dealer.dealername,
                            dealeremail = dealer.dealeremail,
                            updateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                            dealerid = dealerId
                        }, transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception("Dealer cannot be updated.");
                    }

                    await transaction.CommitAsync();
                    return dealer;
                }
            }
        }

        public Dealer GetDealerByNameAndEmail(string name, string email)
        {
            using(var connection = GetConnection())
            {
                var dealer = connection.QueryFirstOrDefault<Dealer>("SELECT * FROM Dealer WHERE dealername = @dealername AND dealeremail = @dealeremail", new{ dealername = name, dealeremail = email});
                return dealer;
            }
        }

        private SQLiteConnection  GetConnection()
        {
            return new SQLiteConnection (_configuration.GetConnectionString(Constants.CONN_STRING_SECTION));
        }
    }
}