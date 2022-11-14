namespace LogisticApi.Models.LogisticModels
{
    public class Bus : ITransport
    {
        public Route Route { get; set; } = new Route();
        public int WeightKg { get; set; }
        public int MaxPassengers { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public int AmountOfPassengers { get { return Passengers.Count; } private set { AmountOfPassengers = value; } }
    }
}
