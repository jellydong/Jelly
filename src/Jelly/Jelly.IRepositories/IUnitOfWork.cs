using System.Threading.Tasks;

namespace Jelly.IRepositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}