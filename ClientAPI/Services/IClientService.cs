using ClientAPI.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Services {
    public interface IClientService {
        Task<TimedGetClientResponse> GetClientById(string id, CancellationToken cancellationToken1);
        void PushEventsToClientStream(string id, CancellationToken cancellationToken);
    }
}