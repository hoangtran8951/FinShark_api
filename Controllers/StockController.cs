using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Stock;
using apiSTockapi.Helpers;
using apiSTockapi.Interfaces;
using apiSTockapi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace apiSTockapi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fMPService;
        public StockController(IStockRepository stockRepo, IFMPService fMPService)
        {
            _stockRepo = stockRepo;
            _fMPService = fMPService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) // Get all Stocks
        {
            var stocksList = await _stockRepo.GetAllAsync(query);
            var stockDtsList = stocksList.Select(s => s.ToStockDto());
            return Ok(stockDtsList);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) // Get stock by Id
        {
            var stockModel = await _stockRepo.GetById(id);
            if(stockModel == null)
                return BadRequest("this stock doesn't exist");
            return Ok(stockModel.ToStockDto());
        }
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetBySymbol([FromRoute] string symbol)
        {
             var stockModel = await _stockRepo.GetBySymbol(symbol);
            if(stockModel == null)
                return BadRequest("this stock doesn't exist");
            return Ok(stockModel.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto) // Add new Stock
        {
            var stockModel = stockDto.FromCreateDtoToModel();
            if(await _stockRepo.GetBySymbol(stockModel.Symbol) != null)
            {
                return BadRequest("this stock symbol already exists, please use another one.");
            }
            await _stockRepo.CreateAsync(stockModel);
            return Ok(stockModel.ToStockDto());
        } 
        [HttpPost("{symbol}")]
        public async Task<IActionResult> CreateBySymbol([FromRoute] string symbol) // create new Stock By Symbol with FMP
        {
            if(await _stockRepo.GetBySymbol(symbol) != null)
            {
                return BadRequest("This stock is already in the database.");
            }
            var stockModel = await _fMPService.GetStockInfoFromSymbol(symbol.ToUpper());
            if(stockModel == null)
                return BadRequest("This Stock's symbol not exist");
            stockModel = await _stockRepo.CreateAsync(stockModel);
            if(stockModel == null)
                return StatusCode(500, "Can not create this Stock");

            return Ok(stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
        {
            var stockModel = updateStockDto.FromUpdateDtoToModel(id);
            stockModel = await _stockRepo.UpdateAsync(stockModel);
            if(stockModel == null)
                return StatusCode(500, "Can not update Stock");
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            var checkStatue = await _stockRepo.DeleteById(id);
            if(checkStatue == false)
                return StatusCode(500, "Can not delete Stock");
            return NoContent();
        }
        
    }
}