using BiiGBackend.Models.SharedModels;
using System.Text.Json;

namespace BiiGBackend.ApplicationCore.Services.GeoPlaces
{
    public class GeoPlacesService
    {
        private readonly List<GeoProperty> Countries;
        private readonly List<GeoProperty> States;
        private readonly List<string[]> Cities;
        public GeoPlacesService()
        {
            string countriesString = System.IO.File.ReadAllText("./PlacesSrc/country.json");
            string stateString = System.IO.File.ReadAllText("./PlacesSrc/state.json");
            string citiesString = System.IO.File.ReadAllText("./PlacesSrc/city.json");
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            // Deserialize the JSON into a Person object
            Countries = JsonSerializer.Deserialize<List<GeoProperty>>(countriesString, options);
            States = JsonSerializer.Deserialize<List<GeoProperty>>(stateString, options);
            Cities = JsonSerializer.Deserialize<List<string[]>>(citiesString, options);

        }
        public async Task<ResponseModal> GetAllCountries()
        {
            return ResponseModal.Send(Countries);
        }

        public async Task<ResponseModal> GetAllState(string countryCode)
        {
            var selectedStates = States.Where(u => u.CountryCode == countryCode).ToList();
            return ResponseModal.Send(selectedStates);
        }

        public async Task<ResponseModal> GetAllCities(string countryCode, string stateCode)
        {
            var selectedStates = Cities.Where(u => u[1] == countryCode && u[2] == stateCode).Select(u => u[0]).ToList();
            return ResponseModal.Send(selectedStates);
        }

        /*
		 {string[5]}
		 */
    }
}

public class GeoProperty
{
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public string? CountryCode { get; set; }
}