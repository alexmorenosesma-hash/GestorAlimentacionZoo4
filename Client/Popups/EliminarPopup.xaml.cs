using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EliminarPopup : Popup<Confirmar>
{
	public EliminarPopup(EliminarPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}