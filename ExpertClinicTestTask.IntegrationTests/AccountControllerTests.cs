using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ExpertClinicTestTask.Domain.Core.Account;
using ExpertClinicTestTask.Domain.Core.Dtos.Account;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;

namespace ExpertClinicTestTask.IntegrationTests;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
   private readonly HttpClient _client;

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = GetBasicAuthHeader("Artem", "Yarochkin");
        }

        [Fact]
        public async Task CreateUser_ValidData_ReturnsOk()
        {
            // Arrange
            var userDto = new UserCreationDto
            {
                LastName = "Yarochkin",
                FirstName = "Artem",
                Patronymic = "Alexandrovich",
                DateOfBirth = DateTime.Now,
                PassportNumber = "6017 066186",
                PlaceOfBirth = "Moscow",
                PhoneNumber = "79604708515",
                Email = "yarochkin.artem@yandex.ru",
                RegistrationAddress = "Moscow",
                LivingAddress = "Moscow"
            };
            var jsonContent = JsonSerializer.Serialize(userDto);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add("x-Device", "mail");

            // Act
            var response = await _client.PostAsync("/api/v1/account", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_MissingDeviceHeader_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new UserCreationDto
            {
            };
            
            var jsonContent = JsonSerializer.Serialize(userDto);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/account", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetUser_ExistingUser_ReturnsOk()
        {
            // Arrange
            var userId = 3;

            // Act
            var response = await _client.GetAsync($"/api/v1/account/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(content);
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);

        }

        [Fact]
        public async Task GetUser_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;

            // Act
            var response = await _client.GetAsync($"/api/v1/account/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SearchUsers_ByLastName_ReturnsOk()
        {
            // Arrange
            var lastName = "Yarochkin";

            // Act
            var response = await _client.GetAsync($"/api/v1/account?lastName={lastName}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(content);
            Assert.NotNull(users);
            Assert.True(users.Any(u => u.LastName == lastName));
        } 
        
        private static AuthenticationHeaderValue GetBasicAuthHeader(string username, string password)
        {
            var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            return new AuthenticationHeaderValue("Basic", authString);
        }

}