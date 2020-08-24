using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jelly.IServices;
using Jelly.Models;
using Jelly.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jelly.Api.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public ValuesController(ILogger<ValuesController> logger, IMapper mapper,IPostService postService)
        {
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
        }
        /// <summary>
        /// 测试AutoMapper
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<PostResource> Get()
        {
            var data = _postService.GetList();
            var postList = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(data);
            return postList;
        }
    }
}
