using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiSTockapi.Models
{
    [Table("Stocks")]
    public class StockModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; }= string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap {get; set; }
        public List<CommentModel> Comments {get; set; } = new List<CommentModel>();
        public List<PortfolioModel> Portfolios {get; set; } = new List<PortfolioModel> (); 
    }
}