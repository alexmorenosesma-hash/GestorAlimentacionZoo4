using Client.Pagemodel.Menu;

namespace Client.Pages.Menu;

public partial class MenuPage : ContentPage
{
	public MenuPage(MenuPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}