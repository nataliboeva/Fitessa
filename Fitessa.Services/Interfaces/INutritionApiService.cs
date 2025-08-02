namespace Fitessa.Services.Interfaces
{
    public interface INutritionApiService
    {
        Task<NutritionInfo> GetNutritionInfoAsync(string foodName);
        Task<List<FoodItem>> SearchFoodAsync(string query);
        Task<NutritionInfo> GetNutritionByBarcodeAsync(string barcode);
    }

    public class NutritionInfo
    {
        public string Name { get; set; } = string.Empty;
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fat { get; set; }
        public decimal Fiber { get; set; }
        public decimal Sugar { get; set; }
        public string ServingSize { get; set; } = string.Empty;
    }

    public class FoodItem
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Calories { get; set; }
    }
} 