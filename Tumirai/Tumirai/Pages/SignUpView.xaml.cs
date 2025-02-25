namespace Tumirai.Pages;

public partial class SignUpView : ContentPage
{

	public SignUpView(SignUpViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel; 
	}
    private async void OnSignInTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignIn");
    }
}