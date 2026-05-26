using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class AlimentoPopup : Popup<Alimento>
{
	public AlimentoPopup(AlimentoPopupPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}