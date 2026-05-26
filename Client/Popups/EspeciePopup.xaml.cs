using Client.Pagemodel.Inventarios;
using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EspeciePopup : Popup<Especie>
{
	public EspeciePopup(EspeciePopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}