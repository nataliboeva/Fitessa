using Fitessa.Services.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Fitessa.Services.Services
{
    public class NutritionApiService : INutritionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NutritionApiService> _logger;

        public NutritionApiService(HttpClient httpClient, ILogger<NutritionApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<NutritionInfo> GetNutritionInfoAsync(string foodName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.edamam.com/api/food-database/v2/parser?app_id=YOUR_APP_ID&app_key=YOUR_APP_KEY&ingr={Uri.EscapeDataString(foodName)}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return ParseNutritionResponse(content);
                }
                
                return GetMockNutritionInfo(foodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching nutrition info for {FoodName}", foodName);
                return GetMockNutritionInfo(foodName);
            }
        }

        public async Task<List<FoodItem>> SearchFoodAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.edamam.com/api/food-database/v2/parser?app_id=YOUR_APP_ID&app_key=YOUR_APP_KEY&ingr={Uri.EscapeDataString(query)}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return ParseFoodSearchResponse(content);
                }
                
                return GetMockFoodItems(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching food for {Query}", query);
                return GetMockFoodItems(query);
            }
        }

        public async Task<NutritionInfo> GetNutritionByBarcodeAsync(string barcode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return ParseBarcodeResponse(content);
                }
                
                return GetMockNutritionInfo("Unknown Product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching nutrition by barcode {Barcode}", barcode);
                return GetMockNutritionInfo("Unknown Product");
            }
        }

        private NutritionInfo ParseNutritionResponse(string jsonResponse)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;
                
                if (root.TryGetProperty("parsed", out var parsed) && parsed.GetArrayLength() > 0)
                {
                    var food = parsed[0];
                    var nutrients = food.GetProperty("food").GetProperty("nutrients");
                    
                    return new NutritionInfo
                    {
                        Name = food.GetProperty("food").GetProperty("label").GetString() ?? "",
                        Calories = nutrients.GetProperty("ENERC_KCAL").GetDecimal(),
                        Protein = nutrients.GetProperty("PROCNT").GetDecimal(),
                        Carbohydrates = nutrients.GetProperty("CHOCDF").GetDecimal(),
                        Fat = nutrients.GetProperty("FAT").GetDecimal(),
                        Fiber = nutrients.GetProperty("FIBTG").GetDecimal(),
                        Sugar = nutrients.GetProperty("SUGAR").GetDecimal(),
                        ServingSize = "100g"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing nutrition response");
            }
            
            return GetMockNutritionInfo("Unknown");
        }

        private List<FoodItem> ParseFoodSearchResponse(string jsonResponse)
        {
            var foodItems = new List<FoodItem>();
            
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;
                
                if (root.TryGetProperty("hints", out var hints))
                {
                    foreach (var hint in hints.EnumerateArray().Take(10))
                    {
                        var food = hint.GetProperty("food");
                        foodItems.Add(new FoodItem
                        {
                            Name = food.GetProperty("label").GetString() ?? "",
                            Brand = food.GetProperty("brand").GetString() ?? "",
                            Barcode = food.GetProperty("foodId").GetString() ?? "",
                            Calories = food.GetProperty("nutrients").GetProperty("ENERC_KCAL").GetDecimal()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing food search response");
            }
            
            return foodItems.Count > 0 ? foodItems : GetMockFoodItems("Unknown");
        }

        private NutritionInfo ParseBarcodeResponse(string jsonResponse)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;
                
                if (root.GetProperty("status").GetInt32() == 1)
                {
                    var product = root.GetProperty("product");
                    var nutriments = product.GetProperty("nutriments");
                    
                    return new NutritionInfo
                    {
                        Name = product.GetProperty("product_name").GetString() ?? "",
                        Calories = nutriments.GetProperty("energy-kcal_100g").GetDecimal(),
                        Protein = nutriments.GetProperty("proteins_100g").GetDecimal(),
                        Carbohydrates = nutriments.GetProperty("carbohydrates_100g").GetDecimal(),
                        Fat = nutriments.GetProperty("fat_100g").GetDecimal(),
                        Fiber = nutriments.GetProperty("fiber_100g").GetDecimal(),
                        Sugar = nutriments.GetProperty("sugars_100g").GetDecimal(),
                        ServingSize = "100g"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing barcode response");
            }
            
            return GetMockNutritionInfo("Unknown Product");
        }

        private NutritionInfo GetMockNutritionInfo(string foodName)
        {
            return new NutritionInfo
            {
                Name = foodName,
                Calories = 150,
                Protein = 5.0m,
                Carbohydrates = 25.0m,
                Fat = 3.0m,
                Fiber = 2.0m,
                Sugar = 8.0m,
                ServingSize = "100g"
            };
        }

        private List<FoodItem> GetMockFoodItems(string query)
        {
            return new List<FoodItem>
            {
                new FoodItem { Name = $"{query} - Organic", Brand = "Organic Brand", Calories = 120 },
                new FoodItem { Name = $"{query} - Premium", Brand = "Premium Brand", Calories = 180 },
                new FoodItem { Name = $"{query} - Natural", Brand = "Natural Brand", Calories = 95 }
            };
        }
    }
} 