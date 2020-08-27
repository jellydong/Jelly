using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.Extensions.Logging;

namespace Jelly.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly TestUserStore _users;
        private readonly ILogger<TestUserProfileService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserProfileService"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="logger">The logger.</param>
        public ProfileService(TestUserStore users, ILogger<TestUserProfileService> logger)
        {
            _users = users;
            _logger = logger;
        }
        /// <summary>
        /// 只要有关用户的身份信息单元被请求（例如在令牌创建期间或通过用户信息终点），就会调用此方法
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);

            #region 根据请求的返回
            ////判断是否有请求Claim信息
            //if (context.RequestedClaimTypes.Any())
            //{
            //    //根据用户唯一标识查找用户信息
            //    var user = _users.FindBySubjectId(context.Subject.GetSubjectId());
            //    if (user != null)
            //    {
            //        //调用此方法以后内部会进行过滤，只将用户请求的Claim加入到 context.IssuedClaims 集合中 这样我们的请求方便能正常获取到所需Claim

            //        context.AddRequestedClaims(user.Claims);
            //    }
            //} 
            #endregion
            var user = _users.FindBySubjectId(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.IssuedClaims.AddRange(user.Claims);
            }
            context.LogIssuedClaims(_logger);

            return Task.CompletedTask;
        }
        /// <summary>
        /// 验证用户是否有效 例如：token创建或者验证
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("IsActive called from: {caller}", context.Caller);

            var user = _users.FindBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = user?.IsActive == true;

            return Task.CompletedTask;
        }
    }
}