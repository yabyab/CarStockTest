using System.Data.SQLite;
using Dapper;
using WebApplication1.Models;

namespace WebApplication1.Repositories{

    public class DealerCarStockRepository : IDealerCarStockRepository
    {
        private readonly IConfiguration _configuration;

        public DealerCarStockRepository(IConfiguration configuration){
            _configuration = configuration;
        }

        public async Task DeleteDealerCarStockAsync(int dealerId, int stockId)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    var existRec = await connection.QuerySingleOrDefaultAsync<DealerCarStock>(
                        @"SELECT * 
                        FROM DealerCarStock
                        WHERE stockid = @id;", new {id = stockId}, transaction);
                    if(existRec == null){
                            await transaction.RollbackAsync();
                            throw new ArgumentException(@"Given car stock id has cannot be found in the stock.", "stockId");
                    }else{
                        if(existRec.dealerid != dealerId){
                            await transaction.RollbackAsync();
                            throw new InvalidDataException(@"Given car stock is not owned by given dealer");
                        }
                    }

                    int recCnt = await connection.ExecuteAsync(
                        @"DELETE FROM DealerCarStock 
                        WHERE stockid = @id", new {id = stockId}, transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception("Dealer car stock cannot be Deleted.");
                    }
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<List<DealerCarStock>> GetAllDealerCarStockByDealerIdAsync(int dealerId)
        {
            using(var connection = GetConnection()){
                var sql = GetDealeCarStockSql(1);
                var dealerCarStocks = await connection.QueryAsync<DealerCarStock>(sql, new {id = dealerId});
                return dealerCarStocks.ToList();
            }
        }

        public async Task<DealerCarStock> GetDealerCarStockByStockIdAsync(int stockId)
        {
            using(var connection = GetConnection()){
                var sql = GetDealeCarStockSql(2);
                var dealerCarStocks = await connection.QuerySingleOrDefaultAsync<DealerCarStock>(sql, new {id = stockId});
                return dealerCarStocks;
            }
        }

        public async Task<List<DealerCarStock>> SearchDealerCarStockAsync(int dealerId, string make, string model, int year)
        {
            string cond = "";
            string yearStr = "";
            if(make != ""){
                make = String.Format(@"%{0}%", make);
                cond += String.Format(@"dcs.make LIKE @make", make);
            }
            if(model != ""){
                if(cond != ""){
                    cond += " OR ";
                }
                model = String.Format(@"%{0}%", model);
                cond +=  String.Format(@"dcs.model LIKE @model", model);
            }
            if(year >= 0){
                if(cond != ""){
                    cond += " OR ";
                }
                cond +=  String.Format(@"dcs.year >= @year",year);
            }
            cond = " (" + cond + ")";
            using(var connection = GetConnection()){
                
                var sql = GetDealeCarStockSql(1, cond);
                var dealerCarStocks = await connection.QueryAsync<DealerCarStock>(sql, new {id = dealerId, make = make, model = model, year = year});
                if(dealerCarStocks == null ||
                    dealerCarStocks.ToList().Count() == 0){
                        throw new ArgumentException(@"No Car stock is found by given make and model");
                    }
                else{
                    return dealerCarStocks.ToList();
                }
            }
        }

        public async Task<int> InsertDealerCarStockAsync(DealerCarStock dealerCarStock)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    int? recCnt = await connection.QuerySingleOrDefaultAsync<int>(
                        @"SELECT count(stockid) 
                        FROM DealerCarStock 
                        WHERE dealerid = @dealerid AND model = @model AND make = @make AND year = @year",
                        dealerCarStock,
                        transaction);
                    if(recCnt > 0){
                        await transaction.RollbackAsync();
                        throw new InvalidDataException("Car stock has already exist.");
                    }
                    string sqlStr = @"
                    INSERT INTO DealerCarStock (dealerid, model, make, year, price, quantity) 
                    VALUES (@dealerid, @model, @make, @year, @price, @quantity);
                    SELECT last_insert_rowid() AS stockid FROM DealerCarStock LIMIT 1;";
                    int resStockId = await connection.QueryFirstAsync<int>(sqlStr, 
                    dealerCarStock,
                    transaction);

                    if(resStockId < 1){
                        await transaction.RollbackAsync();
                        throw new Exception(@"Dealer car stock cannot be created.");
                    }
                    await transaction.CommitAsync();
                    return resStockId;
                }
            }
        }

        public async Task<DealerCarStock> UpdateDealerCarStockInfoAsync(int dealerId, DealerCarStockInfoAdjRequest dealerCarStockInfoAdjRequest)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    int? recCnt = null;
                    var existRec = await connection.QuerySingleOrDefaultAsync<DealerCarStock>(
                        @"SELECT * 
                        FROM DealerCarStock
                        WHERE stockid = @stockid;", dealerCarStockInfoAdjRequest, transaction);
                    if(existRec == null){
                            await transaction.RollbackAsync();
                            throw new ArgumentException(@"Given car stock id has cannot be found in the stock.", "stockId");
                    }else{
                        if(existRec.dealerid != dealerId){
                            await transaction.RollbackAsync();
                            throw new InvalidDataException(@"Given car stock is not owned by given dealer");
                        }
                    }
                    
                    recCnt = await connection.QuerySingleOrDefaultAsync<int>(
                        @"SELECT count(stockid)
                        FROM DealerCarStock
                        WHERE dealerid = @id AND make = @make AND model = @model AND year = @year",
                        new{
                            id = dealerId,
                            make =  dealerCarStockInfoAdjRequest.make,
                            model = dealerCarStockInfoAdjRequest.model,
                            year = dealerCarStockInfoAdjRequest.year
                        }, transaction);
                    if(recCnt > 0) {
                            await transaction.RollbackAsync();
                            throw new InvalidDataException(@"Same car stock has already been used in the stock.");
                    }

                    recCnt = await connection.ExecuteAsync(
                        @"UPDATE DealerCarStock 
                        SET make = @make, model = @model, year = @year, update_at = @updateTime
                        WHERE stockid = @stockid", 
                        new{
                            make = dealerCarStockInfoAdjRequest.make,
                            model = dealerCarStockInfoAdjRequest.model,
                            year = dealerCarStockInfoAdjRequest.year,
                            updateTime=DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                            stockid = dealerCarStockInfoAdjRequest.stockid
                        }, transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception(@"Dealer car stock info cannot be updated.");
                    }

                    await transaction.CommitAsync();
                    existRec.make = dealerCarStockInfoAdjRequest.make;
                    existRec.model = dealerCarStockInfoAdjRequest.model;
                    existRec.year = dealerCarStockInfoAdjRequest.year;
                    return existRec;
                }
            }
        }

        public async Task<DealerCarStock> UpdateDealerCarStockQtyAsync(int dealerId, DealerCarStockQtyAdjRequest dealerCarStockQtyAdjRequest)
        {
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    if(dealerCarStockQtyAdjRequest.quantity < 0){
                        throw new InvalidDataException(@"Given quantity cannot be less than 0.");
                    }
                    var existRec = await connection.QuerySingleOrDefaultAsync<DealerCarStock>(
                        @"SELECT * 
                        FROM DealerCarStock
                        WHERE stockid = @stockid;", dealerCarStockQtyAdjRequest, transaction);
                    if(existRec == null){
                            await transaction.RollbackAsync();
                            throw new ArgumentException(@"Given car stock id has cannot be found in the stock.", "stockId");
                    }else{
                        if(existRec.dealerid != dealerId){
                            await transaction.RollbackAsync();
                            throw new InvalidDataException(@"Given car stock is not owned by given dealer");
                        }
                    }

                    int recCnt = await connection.ExecuteAsync(
                        @"UPDATE DealerCarStock 
                        SET quantity = @quantity, update_at = @updateTime
                        WHERE stockid = @stockid", 
                        new{
                            quantity = dealerCarStockQtyAdjRequest.quantity,
                            updateTime=DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                            stockid = dealerCarStockQtyAdjRequest.stockid
                        }
                        , transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception(@"Dealer car stock level cannot be updated.");
                    }
                    await transaction.CommitAsync();
                    existRec.quantity = dealerCarStockQtyAdjRequest.quantity;
                    return existRec; 
                }
            }
        }

        public async Task<DealerCarStock> UpdateDealerCarStockPriceAsync(int dealerId, DealerCarStockPriceAdjRequest dealerCarStockPriceAdjRequest){
            using(var connection = GetConnection()){
                await connection.OpenAsync();
                using(var transaction = await connection.BeginTransactionAsync()){
                    if(dealerCarStockPriceAdjRequest.price < 0){
                        throw new InvalidDataException(@"Given price cannot be less than 0.");
                    }
                    var existRec = await connection.QuerySingleOrDefaultAsync<DealerCarStock>(
                        @"SELECT * 
                        FROM DealerCarStock
                        WHERE stockid = @stockid;", dealerCarStockPriceAdjRequest, transaction);
                    if(existRec == null){
                            await transaction.RollbackAsync();
                            throw new ArgumentException(@"Given car stock id has cannot be found in the stock.", "stockId");
                    }else{
                        if(existRec.dealerid != dealerId){
                            await transaction.RollbackAsync();
                            throw new InvalidDataException(@"Given car stock is not owned by given dealer");
                        }
                    }

                    int recCnt = await connection.ExecuteAsync(
                        @"UPDATE DealerCarStock 
                        SET price = @price, update_at = @updateTime
                        WHERE stockid = @stockid", 
                        new{
                            price = dealerCarStockPriceAdjRequest.price,
                            updateTime=DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                            stockid = dealerCarStockPriceAdjRequest.stockid
                        }
                        , transaction);
                    if(recCnt < 1){
                        await transaction.RollbackAsync();
                        throw new Exception(@"Dealer car stock level cannot be updated.");
                    }
                    await transaction.CommitAsync();
                    existRec.price = dealerCarStockPriceAdjRequest.price;
                    return existRec; 
                }
            }
        }

        private SQLiteConnection  GetConnection()
        {
            return new SQLiteConnection (_configuration.GetConnectionString(Constants.CONN_STRING_SECTION));
        }

        private string GetDealeCarStockSql(int withWhereClause, string customCondition = "")
        {
            var sql = @"
                    SELECT dcs.*
                    FROM DealerCarStock AS dcs
                    LEFT JOIN Dealer d ON dcs.dealerid = d.dealerid";
            switch(withWhereClause){
                case 1: // Selected by dealerid
                    sql += " WHERE dcs.dealerid = @id";
                    break;
                case 2: // Selected by stockid
                    sql += " WHERE dcs.stockid = @id";
                    break;
                default:
                    break;
            }
            
            if(customCondition != ""){
                    sql += " AND" + customCondition + ";";
            }else{
                sql += ";";
            }

            return sql;
        }
    }
}