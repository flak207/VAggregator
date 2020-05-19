using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KEA.VAggregator.Views;
using KEA.VAggregator.ViewModels;
using KEA.VAggregator.StdLib.Models;
using KEA.VAggregator.StdLib.Services;

namespace KEA.VAggregator.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        private readonly IVideoService _videoService = new TestVideoService();

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;

            var categories = _videoService.GetCategories();
            foreach (var category in categories)
            {
                Image image = new Image()
                {
                    Source = category.ImageUrl, 
                    Aspect = Aspect.AspectFill,
                    WidthRequest = 200,
                    HeightRequest = 100
                };
                (categoryPanel.Children as ICollection<View>).Add(image);
            }
        }
    }
}