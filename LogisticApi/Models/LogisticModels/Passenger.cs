namespace LogisticApi.Models.LogisticModels
{
    public class Passenger
    {
        public Passenger(Station start, Station end, bool haveTicket)
        {
            this.Start = start;
            this.End = end;
            this.HaveTicket = haveTicket;
        }

        public bool HaveTicket { get; set; }
        public Station Start { get; set; }
        public Station End { get; set; }
    }
}
