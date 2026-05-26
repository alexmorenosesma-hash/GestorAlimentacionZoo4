using Client.Pagemodel.Inventarios;
using Client.Pagemodel.Popups;
using CommunityToolkit.Maui.Views;
using Domain.Entities;

namespace Client.Popups;

public partial class HorarioPopup : Popup<Horario>
{
    public HorarioPopup(HorarioPopupPagemodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}