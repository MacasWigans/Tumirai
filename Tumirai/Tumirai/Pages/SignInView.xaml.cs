namespace Tumirai.Pages;

public partial class SignInView : ContentPage
{


	public SignInView(SignInViewModel viewModel)
	{
        InitializeComponent();

		BindingContext = viewModel;
	}

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("//SignUp");
    }

}