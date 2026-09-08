using MediDesk.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MediDesk.Client.Services {
    public class PrescriptionService {
        private readonly HttpClient _httpClient;

        public PrescriptionService() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri("http://localhost:5200/")
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
        public async Task<bool> CreatePrescriptionAsync(
            Prescription prescription) {
            var response = await _httpClient.PostAsJsonAsync(
                "api/prescriptions",
                prescription
            );

            return response.IsSuccessStatusCode;
        }
    }
}