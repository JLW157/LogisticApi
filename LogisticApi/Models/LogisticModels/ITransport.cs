namespace LogisticApi.Models.LogisticModels
{
    public interface ITransport
    {
        public Route Route { get; set; }
        public int WeightKg { get; set; }
        public int MaxPassengers { get; set; }
        public List<Passenger> Passengers { get; set; }
        public int AmountOfPassengers { get { return this.Passengers.Count; } private set { this.AmountOfPassengers = value; } }
    }
}
