using TinderApp.ViewModels;
namespace TinderApp.Views;

public partial class LikePage : ContentPage
{
	public LikePage(LikeViewModel likeViewModel)
	{
		InitializeComponent();
		BindingContext=likeViewModel;
	}
}