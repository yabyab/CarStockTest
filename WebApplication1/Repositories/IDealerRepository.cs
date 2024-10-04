using WebApplication1.Models;
namespace WebApplication1.Repositories
{
    public interface IDealerRepository
    {
        // Dealer GetDealerByNameAndEmail(string name, string email);

        Task<Dealer> GetByIdAsync(int dealerId);
        Task<int> InsertDealerAsync(Dealer dealer);
        Task UpdateDealerAsync(Dealer dealer);
    }
}