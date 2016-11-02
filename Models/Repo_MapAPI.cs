using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using GGeocodeModel;
using GStaticMapAPI;
using GAPI;

public interface IGoogleGeocoding {
    bool isRepeatSearch (string searchTerm);
    Task queryGeoLocationAPIAsync (string searchTerm);
    Task<string> createStaticMap (string searchTerm, string size, int zoom, string maptype, bool returnMap);
    string createJSON (object o);
    List<GeoLocation> getAll ();
}
public class GeoLocation {
    public string searchTerm { get; set; } = "";
    public double geoLatitude { get; set; } = 0.0d;
    public double geoLongtitude { get; set; } = 0.0d;
    public string staticMapURL { get; set; } = "";
}
public class GeoLocationAPI : IGoogleGeocoding {
    // API Endpoint: http://maps.googleapis.com/maps/api/geocode/
    // API Portal / Home Page: https://developers.google.com/maps/documentation/geocoding/
    // Terms Of Service URL: http://code.google.com/terms.html
    // Docs Home Page URL: https://developers.google.com/maps/documentation/geocoding/
    // Architectural Style: REST
    // Supported Request Formats: URI Query String
    // Supported Response Formats: JSON

    private List<GeoLocation> geoCoordinates = new List<GeoLocation>();  
    //Properties:
    public GoogleGeocode specificModel { get; set; }
    public StaticMapAPI sMapModel { get; set; }
    public string searchTerm { get; set; }

    //Methods:
    public GeoLocationAPI (string searchParams="") { searchTerm = searchParams; }
    public async Task<string> createStaticMap (string search, string size, int zoom, string maptype, bool returnMap=false) {
        Console.WriteLine("in createStaticMap with {returnMap}");
        int indexSearch = geoCoordinates.FindIndex(x => x.searchTerm == search.ToLower());
        Console.WriteLine(indexSearch); // Delete
        int swidth = int.Parse(size.Split(new char[]{'x'}).First().ToString());
        int sheight = int.Parse(size.Split(new char[]{'x'}).Last().ToString());

        // https://maps.googleapis.com/maps/api/staticmap?zoom=16&size=600x300&maptype=roadmap&markers=color:red|label:X|29.736554,-95.389975
        string iURL = string.Format($"https://maps.googleapis.com/maps/api/staticmap?center={search}&zoom={zoom}&size={swidth}x{sheight}&maptype={maptype}&markers=color:blue|label:X|{geoCoordinates[indexSearch].geoLatitude},{geoCoordinates[indexSearch].geoLongtitude}");
        geoCoordinates[indexSearch].staticMapURL = iURL;

        // Create static Map Model
        StaticMapAPI sMapModel = new StaticMapAPI();
        sMapModel.imageUrl = iURL;
        sMapModel.timestamp = DateTime.Now;
        sMapModel.searchTerm = searchTerm;
        sMapModel.zoomLevel = (int)(zoom);
        sMapModel.size = new Size { width = swidth, height = sheight };
        sMapModel.markers = new List<Marker> {
            new Marker {
                color = "blue", 
                label = "X", 
                location = new GStaticMapAPI.Location {
                        lat = geoCoordinates[indexSearch].geoLatitude,
                        lng = geoCoordinates[indexSearch].geoLongtitude
                }}};
        if (returnMap) { return (iURL);}
        return (createJSON(sMapModel));
    }

    public async Task queryGeoLocationAPIAsync (string search) {
        // Method queries google geocode website for search terma and saves the lat/lng in variable.
        string searchURL = string.Format(@"https://maps.googleapis.com/maps/api/geocode/json?address=" + search);
        GoogleGeocode specificModel = await GAPI.API.GetDataAsync<GoogleGeocode>(searchURL);

        // Save searchParams
        geoCoordinates.Add( new GeoLocation {
            searchTerm = search.ToLower(),
            geoLatitude = specificModel.results[0].geometry.location.lat,
            geoLongtitude = specificModel.results[0].geometry.location.lng
        });
        Console.WriteLine("In query.");
    }
    public bool isRepeatSearch (string newSearchTerm){
        if (geoCoordinates.Exists(x => x.searchTerm == newSearchTerm)) {
            return true;
        } else { return false;}
    }
    public string createJSON (Object o) {
        return GAPI.API.ToJSON(o);
    }
    public List<GeoLocation> getAll(){ return geoCoordinates; }
}