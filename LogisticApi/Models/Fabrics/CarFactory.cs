using LogisticApi.Models.LogisticModels;
using Route = LogisticApi.Models.LogisticModels.Route;

namespace LogisticApi.Models.Fabrics
{
    public class CarFactory : TransportFactory
    {
        private readonly Route route;
        private readonly int weightKg;
        private readonly int maxPassengers;
        private readonly List<Passenger> passengers = new List<Passenger>();
        public int amountOfPassengers { get { return passengers.Count; } }

        public CarFactory(Route route, int weightKg, int maxPassengers)
        {
            this.route = route;
            this.weightKg = weightKg;
            this.maxPassengers = maxPassengers;
        }

        public override ITransport CreateTransport()
        {
            Car car = new Car()
            {
                Passengers = passengers,
                Route = route,
                WeightKg = weightKg,
                MaxPassengers = maxPassengers
            };

            return car;
        }
    }
}
