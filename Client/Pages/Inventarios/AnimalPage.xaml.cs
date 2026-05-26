using Client.Pagemodel.Inventarios;

namespace Client.Pages.Inventarios;

public partial class AnimalPage : ContentPage
{
	public AnimalPage(AnimalPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}