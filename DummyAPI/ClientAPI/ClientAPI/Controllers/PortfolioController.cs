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
        public async Task<IActionResult> CreateInvestment(CreateInvestmentRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.CreateInvestment(request, cancellationToken);
                return Ok();
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
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
        public async Task<IActionResult> ChangePrice(ChangePriceRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.ChangePrice(request, cancellationToken);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
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
        public async Task<IActionResult> DepositToPortfolio(DepositToPortfolioRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.DepositToPortfolio(request, cancellationToken);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
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
        public async Task<IActionResult> WithdrawFromInvestment(WithdrawFromInvestmentRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.WithdrawFromInvestment(request, cancellationToken);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(nameof(WithdrawFromPortfolio))]
        public async Task<IActionResult> WithdrawFromPortfolio(WithdrawFromPortfolioRequest request, CancellationToken cancellationToken) {
            try {
                await _portfolioService.WithdrawFromPortfolio(request, cancellationToken);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
        

        public PortfolioController(IPortfolioService portfolioService) {

            _portfolioService = portfolioService;
        }
    }
}
