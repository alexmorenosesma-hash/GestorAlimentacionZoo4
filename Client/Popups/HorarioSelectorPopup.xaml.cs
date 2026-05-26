using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class HorarioSelectorPopup : Popup<HorariosSeleccionados>
{
    public HorarioSelectorPopup(HorarioSelectorPopupPagemodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
