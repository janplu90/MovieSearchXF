using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MovieSearchXF
{
    public class App : Application
    {
        public App()
        {
            var movieSearchPage = new MovieSearchPage();
            var movieSearchNavigationPage = new NavigationPage(movieSearchPage);
            movieSearchNavigationPage.Title = "Movies";

            var topRatedPage = new TopRatedPage();
            var topRatedNavigationPage = new NavigationPage(topRatedPage);
            topRatedNavigationPage.Title = "Top Rated";
        

            var popularPage = new PopularPage();
            var popularNavigationPage = new NavigationPage(popularPage);
            popularNavigationPage.Title = "Popular";

            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(movieSearchNavigationPage);
            tabbedPage.Children.Add(topRatedNavigationPage);
            tabbedPage.Children.Add(popularNavigationPage);

            topRatedPage.TopRated();



            //tabbedPage.CurrentPageChanged += async (sender, e) =>
            //{
            //    if (tabbedPage.CurrentPage.Equals(topRatedNavigationPage))
            //    {
            //        await topRatedPage.TopRated();
            //    }
            //    else if (tabbedPage.CurrentPage.Equals(popularNavigationPage))
            //    {
            //        await popularPage.Popular();
            //    }

            //};



            MainPage = new NavigationPage(tabbedPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
