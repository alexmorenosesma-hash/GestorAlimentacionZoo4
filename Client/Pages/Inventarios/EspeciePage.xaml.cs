using Client.Pagemodel.Inventarios;

namespace Client.Pages.Inventarios;

public partial class EspeciePage : ContentPage
{
	public EspeciePage(EspeciePagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;	
	}
}