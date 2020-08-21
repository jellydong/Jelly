using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Jelly.Models.Database
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public class JellyContextSeed
    {
        public static async Task SeedAsync(JellyContext jellyContext, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                // todo:Only run this if using a real database

                if (!jellyContext.Posts.Any())
                {
                    jellyContext.Posts.AddRange(
                        new List<Post>
                        {
                            new Post
                            {
                                Title = "Post Title 1",
                                Body = "Post Body 1",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 2",
                                Body = "Post Body 2",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 3",
                                Body = "Post Body 3",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 4",
                                Body = "Post Body 4",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 5",
                                Body = "Post Body 5",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 6",
                                Body = "Post Body 6",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 7",
                                Body = "Post Body 7",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            },
                            new Post
                            {
                                Title = "Post Title 8",
                                Body = "Post Body 8",
                                Author = "Dave",
                                CreatedTime = DateTime.Now
                            }
                        }
                    );
                    await jellyContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //失败重试
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<JellyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(jellyContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}