using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Models;

namespace apiSTockapi.Interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<StockModel>> GetUserPortfolioAsync(AppUser appUser);
        public Task<PortfolioModel?> AddPortfolioAsync(PortfolioModel portfolio);
        public Task<PortfolioModel?> RemovePortfolioAsync(AppUser appUser, string symbol); 
    }
}