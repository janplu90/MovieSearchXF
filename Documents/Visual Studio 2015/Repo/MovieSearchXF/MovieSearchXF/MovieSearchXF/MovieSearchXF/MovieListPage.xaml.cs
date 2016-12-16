using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MovieSearchXF
{
    public partial class MovieListPage : ContentPage
    {
        public MovieListPage()
        {
            InitializeComponent();
        }

        private void Listview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            this.Navigation.PushAsync(new MovieExtendedInfoPage() { BindingContext = e.SelectedItem });
        }
    }
}
