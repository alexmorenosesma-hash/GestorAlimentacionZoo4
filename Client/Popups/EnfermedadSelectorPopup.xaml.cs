using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class EnfermedadSelectorPopup : Popup<EnfermedadSeleccionadas>
{
    public EnfermedadSelectorPopup(EnfermedadSelectorPopupPagemodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
