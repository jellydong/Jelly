using Jelly.IRepositories;
using Jelly.Models;
using Jelly.Models.Database;

namespace Jelly.Repositories
{
    public partial class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly JellyContext _jellyContext;

        public PostRepository(JellyContext jellyContext) : base(jellyContext)
        {
            _jellyContext = jellyContext;
        }
    }
}