using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BiiGBackend.ApplicationCore.Services
{
	public class PaystackService : IPaystackService
	{
		private readonly HttpClient _httpClient;
		private readonly string _secretKey;
		private readonly string _baseUrl;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public PaystackService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = new HttpClient();
			_secretKey = configuration["Paystack:SecretKey"];
			_baseUrl = configuration["Paystack:BaseUrl"];
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<string> InitializeTransaction(string email, double amount, Guid reference)
		{
			var url = $"{_baseUrl}/transaction/initialize";
			var content = new
			{
				email = email,
				amount = amount * 100, // Convert to kobo,
				reference = reference,
				callback_url = $"{GetUrl()}/order-summary/{reference}"
				//callback_url = $"https://localhost:4200/order-summary/{reference}"
			};

			var requestContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
			requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);

			var response = await _httpClient.PostAsync(url, requestContent);
			var responseString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
				return jsonResponse.GetProperty("data").GetProperty("authorization_url").GetString();
			}
			else
			{
				throw new HttpRequestException($"Error initializing transaction: {responseString}");
			}
		}


		public async Task<PaystackTransactionVerificationResponse> VerifyTransaction(Guid reference)
		{
			var url = $"{_baseUrl}/transaction/verify/{reference}";



			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);

			var response = await _httpClient.GetAsync(url);
			var responseString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var jsonResponse = JsonSerializer.Deserialize<PaystackTransactionVerificationResponse>(responseString);
				return jsonResponse;
			}
			else
			{
				throw new HttpRequestException($"Error verifying transaction: {responseString}");
			}
		}
		private string GetUrl()
		{
			var request = _httpContextAccessor.HttpContext.Request;
			return $"{request.Scheme}://{request.Host}{request.PathBase}";
		}


	}

	public interface IPaystackService
	{
		Task<string> InitializeTransaction(string email, double amount, Guid reference);
		Task<PaystackTransactionVerificationResponse> VerifyTransaction(Guid reference);
	}

	public class PaystackTransactionVerificationResponse
	{
		public string Status { get; set; }
		public string Message { get; set; }
		public PaystackTransactionData Data { get; set; }
	}

	public class PaystackTransactionData
	{
		public string Reference { get; set; }
		public string Status { get; set; }
		public string Gateway_Response { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public DateTime Paid_At { get; set; }
		public string Channel { get; set; }
		public string IPAddress { get; set; }
	}
}
