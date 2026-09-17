namespace MediDesk.Client.Services {
    public static class ApiSettings {
        public static Uri BaseAddress { get; } = CreateBaseAddress();

        private static Uri CreateBaseAddress() {
            string? configuredAddress =
                Environment.GetEnvironmentVariable("MEDIDESK_API_BASE_URL");
            string address = string.IsNullOrWhiteSpace(configuredAddress)
                ? "http://localhost:5200/"
                : configuredAddress.Trim();

            if (!Uri.TryCreate(address, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp
                    && uri.Scheme != Uri.UriSchemeHttps)) {
                throw new InvalidOperationException(
                    "MEDIDESK_API_BASE_URL은 유효한 HTTP 또는 HTTPS 주소여야 합니다."
                );
            }

            return new Uri(uri.AbsoluteUri.TrimEnd('/') + "/");
        }
    }
}
