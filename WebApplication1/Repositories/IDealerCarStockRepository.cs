using WebApplication1.Models;
namespace WebApplication1.Repositories;

public interface IDealerCarStockRepository
{
    Task<List<DealerCarStock>> GetAllDealerCarStockByDealerIdAsync(int dealerId);
    Task<DealerCarStock> GetDealerCarStockByStockIdAsync(int stockId);
    Task<List<DealerCarStock>> SearchDealerCarStockAsync(int dealerId, string make, string model, int year);
    Task<DealerCarStock> InsertDealerCarStockAsync(DealerCarStock dealerCarStock);
    Task<DealerCarStock> UpdateDealerCarStockInfoAsync(int dealerId, DealerCarStockInfoAdjRequest dealerCarStockInfoAdjRequest);
    Task<DealerCarStock> UpdateDealerCarStockQtyAsync(int dealerId, DealerCarStockQtyAdjRequest dealerCarStockQtyAdjRequest);
    Task<DealerCarStock> UpdateDealerCarStockPriceAsync(int dealerId, DealerCarStockPriceAdjRequest dealerCarStockPriceAdjRequest);
    Task DeleteDealerCarStockAsync(int dealerId, DealerCarStock dealerCarStock);
}
