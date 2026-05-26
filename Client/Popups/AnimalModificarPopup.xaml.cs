using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class AnimalModificarPopup : Popup<Animal>
{
	public AnimalModificarPopup(AnimalModificarPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}