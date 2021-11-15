using EventStore.Client;

namespace ClientAPI.Context {
    public interface IEventStoreContext {
        EventStoreClient GetClient();
    }
}