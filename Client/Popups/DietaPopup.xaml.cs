using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class DietaPopup : Popup<Dieta>
{
	public DietaPopup(DietaPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}