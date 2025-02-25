using Microsoft.Maui.Handlers;
using System.Collections.ObjectModel;
using Tumirai.Models;
using Tumirai.Services;
using Tumirai.ViewModels;


namespace Tumirai.Pages;

public partial class HomePage : ContentPage
{
   
   
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        ModifySearchBar();
        BindingContext = viewModel;
    }

   

    private void ModifySearchBar()
    {
        SearchBarHandler.Mapper.AppendToMapping("CustomSearchIconColor", (handler, view) =>
        {
#if ANDROID
            var context = handler.PlatformView.Context;
            var searchIconId = context.Resources.GetIdentifier("search_mag_icon", "id", context.PackageName);
            if (searchIconId != 0)
            {
                var searchIcon = handler.PlatformView.FindViewById<Android.Widget.ImageView>(searchIconId);
                if (searchIcon != null)
                {
                    searchIcon.SetColorFilter(Android.Graphics.Color.Rgb(172, 157, 185), Android.Graphics.PorterDuff.Mode.SrcIn);
                }
            }
#endif
        });
    }



    private async void OnDeliveryTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeliveryPage());
        //await Shell.Current.GoToAsync("//Delivery");
    }
}
