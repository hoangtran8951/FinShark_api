using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiSTockapi.Dto.Comment
{
    public class CommentDto
    {
        public int Id {get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "The Title must be at least 5 characters long")]
        [MaxLength(280, ErrorMessage = "The Titel cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "The Content must be at least 5 characters long")]
        [MaxLength(280, ErrorMessage = "The Content cannot be over 280 characters")]
        public string content {get; set; } = string.Empty;
        public DateTime Createdon {get; set; } = DateTime.UtcNow;
        public int? StockId {get; set; }
        public string CreatedBy {get; set; } = string.Empty;
    }
}