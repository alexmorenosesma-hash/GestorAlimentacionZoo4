using Client.Pagemodel.Inventarios;

namespace Client.Pages.Inventarios;

public partial class AlimentoPage : ContentPage
{
	public AlimentoPage(AlimentoPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}