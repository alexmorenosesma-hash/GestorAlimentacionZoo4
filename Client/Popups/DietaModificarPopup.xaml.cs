using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class DietaModificarPopup : Popup<Dieta>
{
	public DietaModificarPopup(DietaModificarPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext=vm;
	}
}