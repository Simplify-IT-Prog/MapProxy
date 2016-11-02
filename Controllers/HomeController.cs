using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GGeocodeModel;
using GStaticMapAPI;

[Route("/")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Root(){
        return Ok("Root Directory:" + System.Environment.NewLine + 
            "To perform a map api query, send get request to /map/api/{search}." + System.Environment.NewLine + 
            "To return a map, send get request to /map/{search}");
    }
}

[Route("/map")]
public class IGoogleGeocodingController : Controller {
    //private GeoLocation geoCDS;
    private IGoogleGeocoding listGeoCoordinates;
    public IGoogleGeocodingController( IGoogleGeocoding g) {
        listGeoCoordinates = g;
    }

    [HttpGet()]
    public IActionResult index(string search){
        return Ok($"Please enter a search term in the URL after api/");
    }

    [Route("/map/{search}")]
    public async Task<IActionResult> ShowMap(string search, string size="800x600", int zoom=10, string maptype="roadmap")
    {  // TODO add validation to ensure entered values meet Google's search values.
        if (search != "" ) {
            // Search performed previously?
            if (!listGeoCoordinates.isRepeatSearch(search)) {
                // Not a repeat search = New Search (get lat and lng first).
                await listGeoCoordinates.queryGeoLocationAPIAsync(search);
            }
            bool returnMap = true;
            return Redirect(await listGeoCoordinates.createStaticMap(search, size, zoom, maptype, returnMap));
        } else { return NotFound(); }
    }

    [HttpGet("/api/{search}")]
    public async Task<IActionResult> Search(string search, string size="800x600", int zoom=10, string maptype="roadmap")
    {        
        if (search != "" ) {
            // Search performed previously?
            if (!listGeoCoordinates.isRepeatSearch(search)) {
                // Not a repeat search = New Search (get lat and lng first).
                await listGeoCoordinates.queryGeoLocationAPIAsync(search);
            }
            return Ok(await listGeoCoordinates.createStaticMap(search, size, zoom, maptype, false));
        } else { return NotFound();}
    }
}