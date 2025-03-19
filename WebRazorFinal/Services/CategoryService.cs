using BusinessObject;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace WebRazorFinal.Services
{
    public class CategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
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
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("Categories");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("value", out JsonElement valueElement))
            {
                return JsonSerializer.Deserialize<List<Category>>(valueElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Category>();
            }

            return new List<Category>();
        }

        // 🔵 Lấy thông tin tác giả theo ID
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"Categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>();
            }

            return null;
        }

        // 🟡 Thêm tác giả mới
        public async Task<bool> AddCategoryAsync(Category category)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Categories", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🟠 Cập nhật thông tin tác giả
        public async Task<bool> UpdateCategoryAsync(Category updatedCategory)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedCategory), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Categories/{updatedCategory.CategoryID}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🔴 Xóa tác giả theo ID
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"Categories/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
