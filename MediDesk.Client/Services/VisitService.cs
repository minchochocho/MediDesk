using MediDesk.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MediDesk.Client.Services {
    public class VisitService {
        private readonly HttpClient _httpClient;

        public VisitService() {
            _httpClient = new HttpClient {
                BaseAddress = ApiSettings.BaseAddress
            };
        }

        // 특정 환자의 진료기록 조회
        public async Task<List<Visit>> GetVisitsByPatientAsync(int patientId) {
            var visits = await _httpClient
                .GetFromJsonAsync<List<Visit>>(
                    $"api/visits/patient/{patientId}"
                );

            return visits ?? new List<Visit>();
        }
        // POST 메서드
        public async Task<(bool Success, string? ErrorMessage)> CreateVisitAsync(
            Visit visit) {
            using var response = await _httpClient.PostAsJsonAsync(
                "api/visits",
                visit
            );

            if (response.IsSuccessStatusCode) {
                return (true, null);
            }

            string errorMessage = await ApiErrorReader.ReadAsync(response);
            return (false, errorMessage);
        }
    }
}
