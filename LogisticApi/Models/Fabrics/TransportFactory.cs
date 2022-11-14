using LogisticApi.Models.LogisticModels;

namespace LogisticApi.Models.Fabrics
{
    public abstract class  TransportFactory
    {
        public abstract ITransport CreateTransport();
    }
}
