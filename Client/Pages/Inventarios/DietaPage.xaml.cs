using Client.Pagemodel.Inventarios;
using System.Security.Cryptography.X509Certificates;

namespace Client.Pages.Inventarios;

public partial class DietaPage : ContentPage
{
	public DietaPage(DietaPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}