using ClientAPI.Requests.PortfolioRequests;
using ClientAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase {

        private readonly IPortfolioService _portfolioService;

        [HttpPost(nameof(CreateInvestment))]
        public async Task<IActionResult> CreateInvestment() {
            throw new NotImplementedException();
        }

        [HttpPost(nameof(CreatePortfolio))]
        public async Task<IActionResult> CreatePortfolio(CancellationToken cancellationToken) {
            try {
                
                return Ok(await _portfolioService.CreatePortfolio(cancellationToken));
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut(nameof(ChangePrice))]
        public async Task<IActionResult> ChangePrice() {
            throw new NotImplementedException();
        }

        [HttpPost(nameof(DepositToInvestment))]
        public async Task<IActionResult> DepositToInvestment(DepositToInvestmentRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.DepositToInvestment(request, cancellationToken);
                return Ok();
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(nameof(DepositToPortfolio))]
        public async Task<IActionResult> DepositToPortfolio() {
            throw new NotImplementedException();
        }

        [HttpGet(nameof(GetPortfolio))]
        public async Task<IActionResult> GetPortfolio(Guid portfolioId, CancellationToken cancellationToken) {
            try {
                return Ok(await _portfolioService.BuildPortfolio(portfolioId, cancellationToken));
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(nameof(WithdrawFromInvestment))]
        public async Task<IActionResult> WithdrawFromInvestment() {
            throw new NotImplementedException();
        }

        [HttpPost(nameof(WithdrawFromPortfolio))]
        public async Task<IActionResult> WithdrawFromPortfolio() {
            throw new NotImplementedException();
        }
        

        public PortfolioController(IPortfolioService portfolioService) {

            _portfolioService = portfolioService;
        }
    }
}
