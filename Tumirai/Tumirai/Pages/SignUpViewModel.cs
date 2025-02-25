using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tumirai.Pages
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string selectedRole; // Ensure this property exists

        public SignUpViewModel(FirebaseAuthClient authClient)
        {

            _authClient = authClient;
            Roles = new[] { "User", "Admin", "Driver" }; // Define roles

            // Initialize HttpClient with both Timeout and BaseAddress
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(300), // Set the timeout to 5 minutes (300 seconds)
                BaseAddress = new Uri("http://192.168.1.101:5253/api/") // Replace with your backend URL
            };

        }

        // A collection of roles to bind to the UI
        public string[] Roles { get; }

        [RelayCommand]
        public async Task SignUp()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedRole))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a role before signing up.", "OK");
                    return;
                }

                // Create the user in Firebase Authentication
                var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);

                // Debug log to check selected role
                Debug.WriteLine($"Assigning role: {SelectedRole}");

                // Set custom claims via backend API
                var claims = new Dictionary<string, object>
        {
            { "role", SelectedRole }
        };

                await SetCustomClaimsAsync(userCredential.User.Uid, claims);

                // Show a success message
                await Shell.Current.DisplayAlert("Success", "User registered successfully!", "OK");

                // Navigate to sign-in page after signup
                await Shell.Current.GoToAsync("//SignIn");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error during sign-up: {ex.Message}", "OK");
            }
        }


        private async Task SetCustomClaimsAsync(string uid, Dictionary<string, object> claims)
        {
            var request = new SetCustomClaimsRequest
            {
                Uid = uid,
                Claims = claims
            };

            var response = await _httpClient.PostAsJsonAsync("auth/set-custom-claims", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error setting custom claims: {errorMessage}");
                throw new Exception($"Failed to set custom claims: {errorMessage}");
            }
        }


        public ICommand NavigateSignIn { get; }

        public SignUpViewModel()
        {
            NavigateSignIn = new Command<string>(async (param) => await OnNavigateSignIn(param));
        }

        private async Task OnNavigateSignIn(string parameter)
        {
            Debug.WriteLine("Sign In Label Tapped!");
            if (!string.IsNullOrEmpty(parameter))
            {
                await Shell.Current.GoToAsync(parameter);
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