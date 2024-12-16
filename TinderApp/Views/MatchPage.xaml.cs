using TinderApp.ViewModels;

namespace TinderApp.Views;

public partial class MatchPage : ContentPage
{
	public MatchPage(MatchViewModel matchViewModel)
	{
		InitializeComponent();
		BindingContext=matchViewModel;
	}
}