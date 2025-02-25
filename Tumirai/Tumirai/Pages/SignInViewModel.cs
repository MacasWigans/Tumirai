using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Tumirai.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        public string Username => _authClient.User?.Info?.DisplayName;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            try
            {
                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
                

                // Navigate to the main page after successful login

                await Shell.Current.GoToAsync("//Dashboard");
            }
            catch (Exception ex)
            {
                // Show an error message to the user
                await Application.Current.MainPage.DisplayAlert("Sign In Failed", ex.Message, "OK");
            }
        }


        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
