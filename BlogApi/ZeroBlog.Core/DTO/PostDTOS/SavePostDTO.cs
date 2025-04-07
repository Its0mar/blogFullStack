
using System.ComponentModel.DataAnnotations;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.DTO.PostDTOS
{
    public class SavePostDTO
    {
        public Guid UserID { get; set; }
        public Guid PostID { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}
