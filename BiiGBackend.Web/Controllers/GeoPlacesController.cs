using BiiGBackend.ApplicationCore.Services.GeoPlaces;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
    public class GeoPlacesController : BaseController
    {
        private GeoPlacesService _geoPlacesService;

        public GeoPlacesController(GeoPlacesService geoPlacesService)
        {
            _geoPlacesService = geoPlacesService;
        }
        [HttpGet("get-all-countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            return await _geoPlacesService.GetAllCountries();
        }
        [HttpGet("get-states")]
        public async Task<IActionResult> GetAllState([FromQuery] string countryCode)
        {
            return await _geoPlacesService.GetAllState(countryCode);
        }

        [HttpGet("get-cities")]
        public async Task<IActionResult> GetAllCities([FromQuery] string countryCode, string stateCode)
        {
            return await _geoPlacesService.GetAllCities(countryCode, stateCode);
        }

    }
}
