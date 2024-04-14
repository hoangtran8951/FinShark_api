using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Models;

namespace apiSTockapi.Interfaces
{
    public interface IFMPService
    {
        public Task<StockModel?> GetStockInfoFromSymbol(string symbol);
    }
}