using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace MediDesk.Client.Services {
    public static class ApiErrorReader {
        public static async Task<string> ReadAsync(
            HttpResponseMessage response) {
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                try {
                    string body = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(body);

                    if (document.RootElement.TryGetProperty(
                            "errors", out var errors)
                        && errors.ValueKind == JsonValueKind.Object) {
                        var messages = errors.EnumerateObject()
                            .SelectMany(field =>
                                field.Value.ValueKind == JsonValueKind.Array
                                    ? field.Value.EnumerateArray()
                                        .Where(item =>
                                            item.ValueKind == JsonValueKind.String)
                                        .Select(item => item.GetString())
                                    : Enumerable.Empty<string>())
                            .Where(message =>
                                !string.IsNullOrWhiteSpace(message))
                            .ToArray();

                        if (messages.Length > 0) {
                            return string.Join(
                                Environment.NewLine,
                                messages
                            );
                        }
                    }
                } catch (JsonException) {
                    // JSON이 아닌 오류 응답에는 일반 메시지를 사용합니다.
                }

                return "입력 내용을 확인해주세요.";
            }

            if (response.StatusCode == HttpStatusCode.NotFound) {
                return "요청한 데이터를 찾을 수 없습니다.";
            }

            return "서버에서 요청을 처리하지 못했습니다. 잠시 후 다시 시도해주세요.";
        }
    }
}