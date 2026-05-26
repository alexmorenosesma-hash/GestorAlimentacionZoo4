using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class AlimentoModificarPopup : Popup<Alimento>
{
	public AlimentoModificarPopup(AlimentoModificarPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}