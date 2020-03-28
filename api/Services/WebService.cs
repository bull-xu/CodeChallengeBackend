using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using api.Models.Response;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace api.Services
{
    public class WebService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private const string _url = "https://www.thecocktaildb.com/api/json/v1/1/";


        public WebService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<List<Cocktail>> SearchAsync(string keyword)
        {
            var key = $"webservice:search:{keyword}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                var url = _url + $"filter.php?i={keyword}";
                var result = await RequestAsync(url);
                if (result != null)
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                }
                return result;
            });
        }
        
        public async Task<Cocktail> GetDetailsAsync(int id)
        {
            var key = $"webservice:details:{id}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                var url = _url + $"lookup.php?i={id}";
                var result = await RequestAsync(url);
                if (result != null)
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                }
                return result?.FirstOrDefault();
            });
        }
        
        public async Task<Cocktail> RandomAsync()
        {
            const String url = _url + "random.php";
            return (await RequestAsync(url)).FirstOrDefault();
        }

        private async Task<List<Cocktail>> RequestAsync(string url)
        {
            
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var drinks = JsonConvert.DeserializeObject<SearchResponse>(content);
            return drinks.drinks?.Select(convert).ToList();
        }

        private static Cocktail convert(SearchItem item)
        {
            return new Cocktail
            {
                Id = int.Parse(item.id),
                Name = item.name,
                ImageURL = item.imageURL,
                Instructions = item.instructions,
                Ingredients = new List<String>
                {
                    item.Ingredient1,
                    item.Ingredient2,
                    item.Ingredient3,
                    item.Ingredient4,
                    item.Ingredient5,
                    item.Ingredient6,
                    item.Ingredient7,
                    item.Ingredient8,
                    item.Ingredient9,
                    item.Ingredient10,
                    item.Ingredient11,
                    item.Ingredient12,
                    item.Ingredient13,
                    item.Ingredient14,
                    item.Ingredient15
                }.FindAll(x => String.IsNullOrEmpty(x) == false)
            };
        }

        private class SearchResponse
        {
            public List<SearchItem> drinks { get; set; }
            
        }
        
        private class SearchItem
        {
            [JsonProperty("strDrink")]
            public string name { get; set; }
            [JsonProperty("strDrinkThumb")]
            public string imageURL { get; set; }
            [JsonProperty("idDrink")]
            public string id { get; set; }
            [JsonProperty("strInstructions")]
            public string instructions { get; set; }
            [JsonProperty("strIngredient1")]
            public string Ingredient1 { get; set; }
            [JsonProperty("strIngredient2")]
            public string Ingredient2 { get; set; }
            [JsonProperty("strIngredient3")]
            public string Ingredient3 { get; set; }
            [JsonProperty("strIngredient4")]
            public string Ingredient4 { get; set; }
            [JsonProperty("strIngredient5")]
            public string Ingredient5 { get; set; }
            [JsonProperty("strIngredient6")]
            public string Ingredient6 { get; set; }
            [JsonProperty("strIngredient7")]
            public string Ingredient7 { get; set; }
            [JsonProperty("strIngredient8")]
            public string Ingredient8 { get; set; }
            [JsonProperty("strIngredient9")]
            public string Ingredient9 { get; set; }
            [JsonProperty("strIngredient10")]
            public string Ingredient10 { get; set; }
            [JsonProperty("strIngredient11")]
            public string Ingredient11 { get; set; }
            [JsonProperty("strIngredient12")]
            public string Ingredient12 { get; set; }
            [JsonProperty("strIngredient13")]
            public string Ingredient13 { get; set; }
            [JsonProperty("strIngredient14")]
            public string Ingredient14 { get; set; }
            [JsonProperty("strIngredient15")]
            public string Ingredient15 { get; set; }
        }
        
    }
    
    
    
}