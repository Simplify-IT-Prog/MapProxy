using System;
using System.Collections.Generic;

//Simplified Google Geocode model: only lat and long values are utilized.
namespace GGeocodeModel {
    public class GoogleGeocode {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
    public class Location {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Geometry {
        public Location location { get; set; }
    }
    public class Result {
        public Geometry geometry { get; set; }
    }
} // namespace