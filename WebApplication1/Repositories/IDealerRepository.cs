using WebApplication1.Models;
namespace WebApplication1.Repositories
{
    public interface IDealerRepository
    {

        Task<Dealer> GetByIdAsync(int dealerId);
        Task<int> InsertDealerAsync(Dealer dealer);
        Task<Dealer> UpdateDealerAsync(Dealer dealer, int dealerId);
    }
}