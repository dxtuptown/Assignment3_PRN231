using BusinessObject;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace WebFERazor.Services
{
    public class OrderService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
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
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("Orders");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("value", out JsonElement valueElement))
            {
                return JsonSerializer.Deserialize<List<Order>>(valueElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Order>();
            }

            return new List<Order>();
        }

        // 🔵 Lấy thông tin tác giả theo ID
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"Orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Order>();
            }

            return null;
        }

        // 🟡 Thêm tác giả mới
        public async Task<bool> AddOrderAsync(Order order)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Orders", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🟠 Cập nhật thông tin tác giả
        public async Task<bool> UpdateOrderAsync(Order updatedOrder)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedOrder), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Orders/{updatedOrder.OrderID}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // 🔴 Xóa tác giả theo ID
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"Orders/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
