using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Extensions;
using apiSTockapi.Interfaces;
using apiSTockapi.Mappers;
using apiSTockapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace apiSTockapi.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fMPService;
        public PortfolioController(UserManager<AppUser> userManager, IPortfolioRepository portfolioRepository, IStockRepository stockRepository, IFMPService fMPService)
        {
            _userManager = userManager;
            _portfolioRepo = portfolioRepository;
            _stockRepo = stockRepository;
            _fMPService = fMPService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var Username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(Username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser); 
            // System.Diagnostics.Debug.WriteLine(userPortfolio);
            // foreach(var user in userPortfolio)
            //     System.Diagnostics.Debug.WriteLine(user.Comments);
            if(userPortfolio == null)
                return StatusCode(500, "Can not get user's portfolios");
            return Ok(userPortfolio.Select(s => s.ToStockDto()));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserPortfolio(string symbol)
        {
            var Username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(Username);
            var stockModel = await _stockRepo.GetBySymbol(symbol);
            if(stockModel == null){
                stockModel = await _fMPService.GetStockInfoFromSymbol(symbol);
                if(stockModel == null)
                    return BadRequest("This Stock does not exist");
                else
                    await _stockRepo.CreateAsync(stockModel);
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            if(userPortfolio.Any(p => p.Symbol.ToLower() == stockModel.Symbol.ToLower()))
                return BadRequest("Can not add the existed stock to your portfolio.");
            var portfolioModel = new PortfolioModel{
                AppUserId = appUser.Id,
                StockId = stockModel.Id
            };
            await _portfolioRepo.AddPortfolioAsync(portfolioModel);
            return Ok();
        }
        [HttpDelete]

        public async Task<IActionResult> DeleteUserPortfolio(string symbol)
        {
            var Username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(Username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            var portfolioModel = userPortfolio.FirstOrDefault(p => p.Symbol.ToLower() == symbol.ToLower());
            if(portfolioModel == null)
                return BadRequest("The specified stock is not in your portfolio");
            
            await _portfolioRepo.RemovePortfolioAsync(appUser, symbol);
            return NoContent();
        }
    };
}