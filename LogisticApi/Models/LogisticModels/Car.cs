namespace LogisticApi.Models.LogisticModels
{
    public class Car : ITransport
    {
        public Route Route { get; set; } = new Route();
        public int WeightKg { get; set; }
        public int MaxPassengers { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public int AmountOfPassengers { get { return this.Passengers.Count; } private set { this.AmountOfPassengers = value; } }
    }
}
