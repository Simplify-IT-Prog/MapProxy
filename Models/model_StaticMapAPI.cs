using System;
using System.Collections.Generic;

// Static Map API JSON model

namespace GStaticMapAPI {
    public class StaticMapAPI {
        public string imageUrl { get; set; }
        public DateTime timestamp { get; set; }
        public string searchTerm { get; set; }
        public int zoomLevel { get; set; }
        public Size size { get; set; }
        public List<Marker> markers { get; set; }
    }
    public class Size {
        public int width { get; set; }
        public int height { get; set; }
    }
    public class Location {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Marker {
        public string color { get; set; }
        public string label { get; set; }
        public Location location { get; set; }
    }
}  // namespace