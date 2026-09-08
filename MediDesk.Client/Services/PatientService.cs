using MediDesk.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MediDesk.Client.Services {
    public class PatientService {
        // API 서버와 HTTP 통신을 담당 객체. HTTP 요청
        private readonly HttpClient _httpClient;

        // PatientService가 생성될 때 HttpClient를 초기화
        public PatientService() {
            _httpClient = new HttpClient {
                // 기본 주소 설정
                BaseAddress = new Uri("http://localhost:5200/")
            };
        }

        // 전체 환자 목록을 조회
        public async Task<List<Patient>> GetPatientsAsync() {
            // GET 요청
            var patients = await _httpClient
                .GetFromJsonAsync<List<Patient>>("api/patients");

            // 서버 응답이 null이면 빈 List를 반환
            return patients ?? new List<Patient>();
        }

        // 새로운 환자를 서버에 등록
        public async Task<bool> CreatePatientAsync(Patient patient) {
            // POST 요청
            var response = await _httpClient.PostAsJsonAsync(
                "api/patients",
                patient
            );
            // HTTP 상태 코드가 성공 범위(200~299)인지 확인
            return response.IsSuccessStatusCode;
        }

        // 업데이트
        public async Task<bool> UpdatePatientAsync(int id, Patient patient) {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/patients/{id}",
                patient
            );

            return response.IsSuccessStatusCode;
        }

        // 삭제
        public async Task<bool> DeletePatientAsync(int id) {
            var response = await _httpClient.DeleteAsync(
                $"api/patients/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}