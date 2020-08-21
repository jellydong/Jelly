using System.Threading.Tasks;
using Jelly.Models.Database;

namespace Jelly.Repositories
{
    public class UnitOfWork
    {
        private readonly JellyContext _jellyContext;

        public UnitOfWork(JellyContext jellyContext)
        {
            _jellyContext = jellyContext;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _jellyContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _jellyContext.SaveChanges();
        }
    }
}