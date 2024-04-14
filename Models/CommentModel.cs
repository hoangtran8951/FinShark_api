using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiSTockapi.Models
{
    [Table("Comments")]
    public class CommentModel
    {
        public int Id {get; set; }
        public string Title { get; set; } = string.Empty;
        public string content {get; set; } = string.Empty;
        public DateTime Createdon {get; set; } = DateTime.UtcNow;
        public int? StockId {get; set; }
        public StockModel? Stock { get; set; }
        public string AppUserId {get; set; }
        public AppUser AppUser {get; set;}
    }
}