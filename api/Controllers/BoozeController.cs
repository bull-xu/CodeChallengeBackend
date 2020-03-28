using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Response;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BoozeController : ControllerBase
    {
        private readonly WebService _webService;
        // We will use the public CocktailDB API as our backend
        // https://www.thecocktaildb.com/api.php
        //
        // Bonus points
        // - Speed improvements
        // - Unit Tests

        public BoozeController(WebService webService)
        {
            _webService = webService;
        }
        
        [HttpGet]
        [Route("search-ingredient/{ingredient}")]
        public async Task<IActionResult> GetIngredientSearchAsync([FromRoute] string ingredient)
        {
            var cocktailList = new CocktailList();
            var drinks = await _webService.SearchAsync(ingredient);
            if (drinks == null)
            {
                return Ok(cocktailList);
            }
            
            cocktailList.Cocktails = new List<Cocktail>();
            cocktailList.meta = new ListMeta();

            foreach (var drink in drinks)
            {
                var detail = await _webService.GetDetailsAsync(drink.Id);
                cocktailList.Cocktails.Add(detail);
            }

            cocktailList.meta.count = cocktailList.Cocktails.Count;
            cocktailList.meta.firstId = cocktailList.Cocktails.Min(x => x.Id);
            cocktailList.meta.lastId = cocktailList.Cocktails.Max(x => x.Id);
            cocktailList.meta.medianIngredientCount = cocktailList.Cocktails.Average(x => x.Ingredients.Count);
            
            return Ok(cocktailList);
        }

        [HttpGet]
        [Route("random")]
        public async Task<IActionResult> GetRandom()
        {
            var drink = await _webService.RandomAsync();
            return Ok(drink);
        }
    }
}