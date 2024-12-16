using TinderApp.ViewModels;

namespace TinderApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel mainviewModel)
        {
            InitializeComponent();
            BindingContext = mainviewModel;
        }

    }
}