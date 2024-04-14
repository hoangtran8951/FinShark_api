using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Helpers;
using apiSTockapi.Models;

namespace apiSTockapi.Interfaces
{
    public interface IStockRepository
    {
        public Task<StockModel> CreateAsync(StockModel stock); // create new stock
        public Task<StockModel?> UpdateAsync(StockModel updateStock); // Update existing Stock
        public Task<List<StockModel>> GetAllAsync(QueryObject query); // Get All the existed Stocks
        public Task<StockModel?> GetById(int id); // Get stock by id
        public Task<StockModel?> GetBySymbol(string symbol); // Get stock by it's symbol
        public Task<bool> DeleteById(int id);
    }
}