using SmartEvent.DurableFunction.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Services
{
    public class SeatRegisService: ISeatRegisService
    {
        private readonly HttpClient _httpClient;       

        public SeatRegisService(HttpClient httpClient)
        {
            _httpClient = httpClient;           
        }

        public async Task ReserveAsync(
            string eventId,
            string idempotencyKey,
            SeatRequest request,
            CancellationToken cancellationToken)
        {
            var url = $"/internal/events/{eventId}/seats/reserve";

            var payload = new
            {
                EventId = eventId,
                RegistrationId = request.RegistrationId,
                Quantity = request.Quantity
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            // Add idempotency header
            _httpClient.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);

            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Failed to reserve seats. Status: {response.StatusCode}");
            }
        }
    }
}
