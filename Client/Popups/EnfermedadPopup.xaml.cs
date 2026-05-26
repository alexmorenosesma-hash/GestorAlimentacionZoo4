using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EnfermedadPopup : Popup<Enfermedad>
{
	public EnfermedadPopup(EnfermedadPopupPagemodel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}