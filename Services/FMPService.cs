using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Stock;
using apiSTockapi.Interfaces;
using apiSTockapi.Mappers;
using apiSTockapi.Models;
using Newtonsoft.Json;

namespace apiSTockapi.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<StockModel?> GetStockInfoFromSymbol(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if(result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var jsonToModel = JsonConvert.DeserializeObject<FMPStockDto[]>(content);
                    var stock = jsonToModel[0];
                    if(stock == null)
                        return null;
                    return stock.StockFromFMP();
                } 
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }
    }
}