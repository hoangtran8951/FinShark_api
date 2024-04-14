using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Data;
using apiSTockapi.Interfaces;
using apiSTockapi.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSTockapi.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PortfolioModel?> AddPortfolioAsync(PortfolioModel portfolio)
        {
            await _context.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<StockModel>> GetUserPortfolioAsync(AppUser appUser)
        {
            return await _context.Portfolios.Where(p => p.AppUserId == appUser.Id).
            Select(p => new StockModel
            {
                Id = p.StockId,
                Symbol = p.Stock.Symbol,
                CompanyName = p.Stock.CompanyName,
                Purchase = p.Stock.Purchase,
                LastDiv = p.Stock.LastDiv,
                Industry = p.Stock.Industry,
                MarketCap = p.Stock.MarketCap,
                // Comments = p.Stock.Comments
            }).ToListAsync();
        }

        public async Task<PortfolioModel?> RemovePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(p => p.Stock.Symbol.ToLower() == symbol);
            if(portfolioModel == null)
                return null;
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }
    }
}