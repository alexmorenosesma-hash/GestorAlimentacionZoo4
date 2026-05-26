using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class CuidadorPopup : Popup<Cuidador>
{
	public CuidadorPopup(CuidadorPopupPagemodel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}