using System;

namespace Jelly.Models
{
    public interface IEntity
    {
        int Id { get; set; }
        int DeleteFlag { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        int UpdatedBy { get; set; }
        DateTime UpdatedTime { get; set; }
        string Uuid { get; set; }
    }
}