using TinderApp.ViewModels;
namespace TinderApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext=loginViewModel;
	}
}