namespace LogisticApi.Models.LogisticModels
{

    public class Station
    {
        public Station(double lat, double lon, string name)
        {
            this.Lat = lat;
            this.Lon = lon;
            this.Name = name;
        }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
