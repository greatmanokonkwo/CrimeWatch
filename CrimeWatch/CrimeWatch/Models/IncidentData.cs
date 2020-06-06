using Newtonsoft.Json;

namespace CrimeWatch.Models
{
    public class IncidentData
    { 
        [JsonProperty("total_incidents")]
        public int NumberOfIncidents { get; set; }

        [JsonProperty("incidents")]
        public Incident[] Incidents { get; set; } 
    }

    public class Incident
    {
        [JsonProperty("incident_offense")]
        public string Type { get; set; }

        [JsonProperty("incident_offense_detail_description")]
        public string Description { get; set; }

        [JsonProperty("incident_latitude")]
        public double Latitude { get; set; }

        [JsonProperty("incident_longitude")]
        public double Longitude { get; set; }

        [JsonProperty("incident_address")]
        public string Address { get; set; }

        [JsonProperty("incident_date")]
        public string Time { get; set; }

        public string Icon { get; set; }

        public string StandardTime { get; set; }

    }
}
