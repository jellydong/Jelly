using System;

namespace Jelly.Models
{
    public class Post:Entity
    {
        public string Title { get; set; }
        public string Body { get; set; } 
        public string Author { get; set; } 
    }
}