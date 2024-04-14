using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Comment;

namespace apiSTockapi.Dto.Stock
{
    public class StockDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; }= string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap {get; set; }
        public List<CommentDto> Comments {get; set; } = new List<CommentDto>();
    }
}