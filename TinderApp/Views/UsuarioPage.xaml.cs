using TinderApp.Models;
namespace TinderApp.Views;

public partial class UsuarioPage : ContentPage
{
	public UsuarioPage(UsuarioPage usuarioPage)
	{
		InitializeComponent();
		BindingContext = usuarioPage;
	}
}