using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiSTockapi.Models
{
    [Table("Portfolio")]
    public class PortfolioModel
    {
        public string AppUserId {get; set; }
        public int StockId {get; set; }
        public AppUser AppUser {get; set; }
        public StockModel Stock {get; set; }
    }
}