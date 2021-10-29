using ClientAPI.Entities;
using ClientAPI.Requests.PortfolioRequests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Services {
    public interface IPortfolioService {
        Task ChangePrice(Guid portfolioId, Guid investmentId, double priceChange);
        Task CreateInvestment(Guid portfolioId, string investmentId, double initialInvestment);
        Task DepositToInvestment(DepositToInvestmentRequest request, CancellationToken cancellationToken);
        Task DepositToPortfolio(Guid portfolioId, double deposit);
        Task WithdrawFromInvestment(Guid portfolioId, string investmentId, double withdrawal);
        Task WithdrawFromPortfolio(Guid portfolioId, double withdrawal);
        Task<Guid> CreatePortfolio(CancellationToken cancellationToken);
        Task<Portfolio> BuildPortfolio(Guid portfolioId, CancellationToken cancellationToken);
    }
}