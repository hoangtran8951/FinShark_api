using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Stock;
using apiSTockapi.Models;

namespace apiSTockapi.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this StockModel stock){
            return new StockDto {
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => c.FromModelToDto()).ToList(),
            };
        }
        public static StockModel FromCreateDtoToModel(this CreateStockDto stockDto)
        {
            return new StockModel
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
        public static StockModel FromUpdateDtoToModel(this UpdateStockDto updateStockDto, int id)
        {
            return new StockModel
            {
                Id = id,
                Symbol = updateStockDto.Symbol,
                CompanyName = updateStockDto.CompanyName,
                Purchase = updateStockDto.Purchase,
                LastDiv = updateStockDto.LastDiv,
                Industry = updateStockDto.Industry,
                MarketCap = updateStockDto.MarketCap
            };
        }
        public static StockModel StockFromFMP(this FMPStockDto fMPStock)
        {
            return new StockModel
            {
                Symbol = fMPStock.symbol,
                CompanyName = fMPStock.companyName,
                Purchase = (decimal)fMPStock.price,
                LastDiv = (decimal)fMPStock.lastDiv,
                Industry = fMPStock.industry,
                MarketCap = fMPStock.mktCap
            };
        }
    }
}