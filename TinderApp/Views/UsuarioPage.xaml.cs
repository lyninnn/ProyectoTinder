using TinderApp.Models;
using TinderApp.ViewModels;
namespace TinderApp.Views;

public partial class UsuarioPage : ContentPage
{
	public UsuarioPage(UsuarioViewModel usuarioViewModel)
	{
		InitializeComponent();
		BindingContext = usuarioViewModel;
	}
    
}