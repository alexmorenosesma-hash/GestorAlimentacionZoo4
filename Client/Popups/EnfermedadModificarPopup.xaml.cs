using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EnfermedadModificarPopup : Popup<Enfermedad>
{
	public EnfermedadModificarPopup(EnfermedadModificarPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}