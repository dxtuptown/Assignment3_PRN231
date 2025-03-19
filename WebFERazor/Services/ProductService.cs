using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BusinessObject;

namespace WebFERazor.Services
{
    public class ProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        //Gọi API
        private HttpClient CreateClient()
        {
            var client = _clientFactory.CreateClient("WebAPI");

            // Lấy token từ session
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        // 🟢 Lấy danh sách tất cả tác giả
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("Products");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("value", out JsonElement valueElement))
            {
                return JsonSerializer.Deserialize<List<Product>>(valueElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Product>();
            }

            return new List<Product>();
        }

        // 🔵 Lấy thông tin tác giả theo ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            return null;
        }

        // 🟡 Thêm tác giả mới
        public async Task<bool> AddProductAsync(Product product)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Products", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🟠 Cập nhật thông tin tác giả
        public async Task<bool> UpdateProductAsync(Product updatedProduct)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedProduct), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Products/{updatedProduct.ProductID}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🔴 Xóa tác giả theo ID
        public async Task<bool> DeleteProductAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"Products/{id}");
            return response.IsSuccessStatusCode;
        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("Categories");
            return response.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<List<Category>>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Category>()
                : new List<Category>();
        }
    }
}
