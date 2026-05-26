using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class AlimentoSelectorPopup : Popup
{
    public AlimentoSelectorPopup(AlimentoSelectorPopupPagemodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}