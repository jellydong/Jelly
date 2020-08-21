using Jelly.IRepositories;
using Jelly.IServices;
using Jelly.Models;

namespace Jelly.Services
{
    public partial class PostService : BaseService<Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IPostRepository iPostRepository, IUnitOfWork unitOfWork) : base(iPostRepository)
        {
            _postRepository = iPostRepository;
            _unitOfWork = unitOfWork;
        }

    }
}