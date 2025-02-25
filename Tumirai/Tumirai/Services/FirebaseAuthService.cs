using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Tumirai.Services
{
    using System.Net.Http.Json;

    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;

        public FirebaseAuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://your-backend-url/api/") // Replace with your backend URL
            };
        }

        // Method to set custom claims via the backend API
        public async Task SetCustomClaimsAsync(string uid, Dictionary<string, object> claims)
        {
            var request = new SetCustomClaimsRequest
            {
                Uid = uid,
                Claims = claims
            };

            var response = await _httpClient.PostAsJsonAsync("auth/set-custom-claims", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to set custom claims.");
            }
        }

        // Method to handle user sign-up
        public async Task SignUp(string email, string password, string role)
        {
            try
            {
                // Create the user in Firebase Authentication
                var userCredential = await MauiProgram.AuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

                // Set custom claims via backend API
                var claims = new Dictionary<string, object> { { "role", role } };
                await SetCustomClaimsAsync(userCredential.User.Uid, claims);

                // Navigate to the next page or show a success message
            }
            catch (Exception ex)
            {
                // Handle errors
                Console.WriteLine($"Error during sign-up: {ex.Message}");
            }
        }
    }

    // Request model for setting custom claims
    public class SetCustomClaimsRequest
    {
        public string Uid { get; set; }
        public Dictionary<string, object> Claims { get; set; }
    }
}
