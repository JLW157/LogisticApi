namespace LogisticApi.Models.LogisticModels
{
    public class Route
    {
        public Route(DateTime departureTime)
        {
            this.DepartureTime = departureTime;
        }
        public Route()
        {

        }
        public List<Station> Points { get; set; } = new List<Station>();
        public DateTime DepartureTime { get; set; }
        public double Lenght { get { return CalculateLentgh(); } private set { this.Lenght = value; } }
        private double CalculateLentgh()
        {
            if (this.Points.Count < 1)
            {
                Console.WriteLine("There is no points.");
                return 0;
            }

            double lenght = 0d;

            for (int i = 0; i < Points.Count; i++)
            {
                Station start = this.Points[i];
                for (int j = i + 1; j < i + 2; j++)
                {
                    if (j >= this.Points.Count)
                    {
                        break;
                    }
                    Station end = this.Points[j];
                    lenght += CalcuteDistance(start, end);
                }
            }

            return lenght;
        }

        private double CalcuteDistance(Station start, Station end)
        {
            double lon1 = start.Lon;
            double lat1 = start.Lat;
            double lon2 = end.Lon;
            double lat2 = end.Lat;


            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * 6378.16;
        }
        private double Radians(double x)
        {
            return x * 3.141592653589793 / 180;
        }
    }
}
