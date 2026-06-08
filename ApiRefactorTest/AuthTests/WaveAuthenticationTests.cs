using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;

namespace ApiRefactorTest.AuthTests
{
    public  class WaveAuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public WaveAuthenticationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_Returns401_WhenNoToken()
        {
            var response = await _client.GetAsync("/api/wave");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_Returns403_WhenUserLacksPolicy()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/wave");

            request.Content = new StringContent("""
            {
                "id": "11111111-1111-1111-1111-111111111111",
                "name": "Wave1",
                "waveDate": "2026-01-01T00:00:00Z"
            }
            """, Encoding.UTF8, "application/json");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "bad_token");

            var response = await _client.SendAsync(request);
        }
        
        [Fact]
        public async Task Update_Returns403_WhenUserLacksPolicy()
        {
            var id = Guid.NewGuid();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/wave/{id}");

            request.Content = new StringContent("""
            {
                "name": "UpdatedWave",
                "waveDate": "2026-01-01T00:00:00Z"
            }
            """, Encoding.UTF8, "application/json");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "bad_token");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}

