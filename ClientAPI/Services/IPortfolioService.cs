using ClientAPI.Entities;
using ClientAPI.Requests.PortfolioRequests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Services {
    public interface IPortfolioService {
        Task ChangePrice(ChangePriceRequest request, CancellationToken cancellationToken);
        Task CreateInvestment(CreateInvestmentRequest request, CancellationToken cancellationToken);
        Task DepositToInvestment(DepositToInvestmentRequest request, CancellationToken cancellationToken);
        Task DepositToPortfolio(DepositToPortfolioRequest request, CancellationToken cancellationToken);
        Task WithdrawFromInvestment(WithdrawFromInvestmentRequest request, CancellationToken cancellationToken);
        Task WithdrawFromPortfolio(WithdrawFromPortfolioRequest request, CancellationToken cancellationToken);
        Task<Guid> CreatePortfolio(CancellationToken cancellationToken);
        Task<Portfolio> BuildPortfolio(Guid portfolioId, CancellationToken cancellationToken);
        Task PushEvents(Guid portfolioId, int events, int iterations, CancellationToken cancellationToken);
    }
}