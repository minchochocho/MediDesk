using MediDesk.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace MediDesk.Client.Services {
    public class VisitService {
        private readonly HttpClient _httpClient;

        public VisitService() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri("http://localhost:5200/")
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
        public async Task<bool> CreateVisitAsync(Visit visit) {
            var response = await _httpClient.PostAsJsonAsync(
                "api/visits",
                visit
            );

            if (!response.IsSuccessStatusCode) {
                var error = await response.Content.ReadAsStringAsync();

                MessageBox.Show(
                    $"진료 등록 실패\n" +
                    $"Status: {(int)response.StatusCode} {response.StatusCode}\n\n" +
                    $"Response:\n{error}"
                );
            }

            return response.IsSuccessStatusCode;
        }
    }
}