using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Data;
using apiSTockapi.Helpers;
using apiSTockapi.Interfaces;
using apiSTockapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace apiSTockapi.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockModel> CreateAsync(StockModel stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> DeleteById(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(item => item.Id == id);
            if(stock == null)
                return false;
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StockModel>> GetAllAsync(QueryObject query)
        {
            var stocksList = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            if(!string.IsNullOrEmpty(query.Symbol))
            {
                stocksList = stocksList.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrEmpty(query.CompanyName))
                stocksList = stocksList.Where(s => s.CompanyName.ToLower().Contains(s.CompanyName.ToLower()));
            
            if(!string.IsNullOrEmpty(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                    stocksList = query.IsDescending ? stocksList.OrderByDescending(s => s.Symbol) : stocksList.OrderBy(s => s.Symbol);
            }

            var skipNumber = (query.PageNumber - 1)*query.PageSize;

            return await stocksList.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<StockModel?> GetById(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<StockModel?> GetBySymbol(string symbol)
        {
            return await _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync(item => item.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<StockModel?> UpdateAsync(StockModel updateStock)
        {
            var stockModel = await GetById(updateStock.Id);
            if(stockModel == null)
                return null;
            stockModel.Symbol = updateStock.Symbol;
            stockModel.CompanyName = updateStock.CompanyName;
            stockModel.Purchase = updateStock.Purchase;
            stockModel.LastDiv = updateStock.LastDiv;
            stockModel.Industry = updateStock.Industry;
            stockModel.MarketCap = updateStock.MarketCap;
            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}