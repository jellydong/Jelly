using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jelly.Models;
using Jelly.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jelly.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IMapper _mapper;

        public ValuesController(ILogger<ValuesController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public PostResource Get()
        {
            Post post = new Post()
            {
                Author = "Jelly",
                Body = "文章内容",
                Title = "文章标题",
                UpdatedTime = DateTime.Now
            };
            PostResource postResource = _mapper.Map<Post, PostResource>(post);
            return postResource;
        }
    }
}
