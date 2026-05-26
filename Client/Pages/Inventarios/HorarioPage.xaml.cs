using Client.Pagemodel.Inventarios;
namespace Client.Pages.Inventarios;

public partial class HorarioPage : ContentPage
{

	public HorarioPage(HorarioPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}