using Client.Pagemodel.Inventarios;

namespace Client.Pages.Inventarios;

public partial class CuidadorPage : ContentPage
{
	public CuidadorPage(CuidadorPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}