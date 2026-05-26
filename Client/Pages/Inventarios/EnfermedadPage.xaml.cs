using Client.Pagemodel.Inventarios;

namespace Client.Pages.Inventarios;

public partial class EnfermedadPage : ContentPage
{
	public EnfermedadPage(EnfermedadPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}