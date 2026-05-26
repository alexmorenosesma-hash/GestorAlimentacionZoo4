using Aplication.Interfaces.Repositories;
using Client.Pagemodel.Popups;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EspecieModificarPopup : Popup<Especie>
{
	public EspecieModificarPopup(EspecieModificarPopupPagemodel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}