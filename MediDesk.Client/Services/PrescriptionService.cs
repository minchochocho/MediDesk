using MediDesk.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MediDesk.Client.Services {
    public class PrescriptionService {
        private readonly HttpClient _httpClient;

        public PrescriptionService() {
            _httpClient = new HttpClient {
                BaseAddress = ApiSettings.BaseAddress
            };
        }

        // 특정 진료의 처방 목록 조회
        public async Task<List<Prescription>> GetPrescriptionsByVisitAsync(
            int visitId) {
            var prescriptions = await _httpClient
                .GetFromJsonAsync<List<Prescription>>(
                    $"api/prescriptions/visit/{visitId}"
                );

            return prescriptions ?? new List<Prescription>();
        }

        // 처방 등록
        public async Task<(bool Success, string? ErrorMessage)> CreatePrescriptionAsync(
            Prescription prescription) {
            using var response = await _httpClient.PostAsJsonAsync(
                "api/prescriptions",
                prescription
            );

            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await ApiErrorReader.ReadAsync(response));
        }
    }
}
