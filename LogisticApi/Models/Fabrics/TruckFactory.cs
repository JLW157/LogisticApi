using LogisticApi.Models.LogisticModels;
using Route = LogisticApi.Models.LogisticModels.Route;

namespace LogisticApi.Models.Fabrics
{
    public class TruckFactory : TransportFactory
    {
        private readonly Route route;
        private readonly int weightKg;
        private readonly int maxPassengers;
        private readonly List<Passenger> passengers = new List<Passenger>();
        public int amountOfPassengers { get { return passengers.Count; } }

        public TruckFactory(Route route, int weightKg, int maxPassengers)
        {
            this.route = route;
            this.weightKg = weightKg;
            this.maxPassengers = maxPassengers;
        }

        public override ITransport CreateTransport()
        {
            Truck truck = new Truck()
            {
                Passengers = passengers,
                Route = route,
                WeightKg = weightKg,
                MaxPassengers = maxPassengers
            };

            return truck;
        }
    }
}
