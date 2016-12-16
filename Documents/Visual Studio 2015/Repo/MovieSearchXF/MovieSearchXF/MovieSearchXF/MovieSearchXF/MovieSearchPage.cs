using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MovieSearchXF
{
    using DM.MovieApi;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using MovieSearchXF.Model;

    public class MovieSearchPage : ContentPage
    {

        private ApiSearchResponse<MovieInfo> _responseMovieInfo;
        private IReadOnlyList<MovieInfo> _movieInfo;
        private IApiMovieRequest _movieApi;
        public MoviesObjects _moviesObjects;
        private ActivityIndicator _ai;

        private Label _movieLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Enter movie name:",
            TextColor = Color.Black
        };

        private Entry _movieEntry = new Entry
        {
            HorizontalOptions = LayoutOptions.Fill,
            Placeholder = "Movie",
            TextColor = Color.FromRgb(71, 117, 255),
            FontAttributes = FontAttributes.Bold,
        };

        private Button _searchMovieButton = new Button
        {
            Text = "Search movie",
            BorderColor = Color.Black,
            HorizontalOptions = LayoutOptions.Fill,
        };

        private Label _displayMovieLabel = new Label
        {
            Text = string.Empty,
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        };

        private ActivityIndicator _AI = new ActivityIndicator()
        {
            HorizontalOptions = LayoutOptions.Center,
            Color = Color.Aqua
        };

        public MovieSearchPage()
        {
           // this._moviesmodel = new List<MoviesModel>();
            this._moviesObjects = new MoviesObjects();
            this._ai = new ActivityIndicator();
            _ai.IsVisible = false;
           // this.BackgroundColor = Color.FromRgb(204, 230, 255);
            MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;


            this.Title = "Movie search page";

            this.Content = new StackLayout
            {

                Margin = 30,
                VerticalOptions = LayoutOptions.Start,
                Spacing = 10,
                Children =
                                       {
                                           new StackLayout { Children = { this._movieLabel, this._movieEntry, }, },
                                           this._searchMovieButton,
                                           this._displayMovieLabel,
                                           this._ai
                                       }
            };

            this._searchMovieButton.Clicked += this.OnDisplayNameButtonClicked;
            this._movieEntry.Completed += this.OnDisplayNameButtonClicked;
        }

        private async void OnDisplayNameButtonClicked(object sender, EventArgs args)
        {
            if (_movieEntry.Text.Length <= 0)
            {
                _searchMovieButton.IsEnabled = true;
                _movieEntry.Text = "Enter movie name!";
            }
            else
            {
              
                _searchMovieButton.IsEnabled = false;
                _ai.IsVisible = true;
                _ai.IsRunning = true;

                this._moviesObjects._moviesModelList.Clear();

                _responseMovieInfo = await _movieApi.SearchByTitleAsync(_movieEntry.Text);
                _movieInfo = _responseMovieInfo.Results;

                for (int i = 0; i < _movieInfo.Count; i++)
                {

                    ApiQueryResponse<MovieCredit> movieInfoCast = await _movieApi.GetCreditsAsync(_movieInfo[i].Id);
                    ApiQueryResponse<Movie> movieInfoGenre = await _movieApi.FindByIdAsync(_movieInfo[i].Id);

                    var movieInfoCastList = movieInfoCast.Item.CastMembers;
                    var movieInfoGenresList = movieInfoGenre.Item.Genres;
                    var movieInfoTime = movieInfoGenre.Item.Runtime;


                    string path = "Not check";
                    if (_movieInfo[i].PosterPath == null)
                    {
                        path = "Empty";
                    }
                    else
                    {
                        path = "http://image.tmdb.org/t/p/w92" +  _movieInfo[i].PosterPath;
                    }

                    string genreList = movieInfoTime + " | ";

                    if (movieInfoGenresList.Count == 0)
                    {
                        genreList += "";
                    }
                    else
                    {
                        genreList += movieInfoGenresList[0].Name;
                    }

                    for (var j = 1; j < movieInfoGenresList.Count; j++)
                    {
                        if (!movieInfoGenresList[j].Equals(null))
                        {
                            genreList += ", " + movieInfoGenresList[j].Name;
                        }
                    }

                    switch (movieInfoCastList.Count)
                    {
                        case 0:
                            this._moviesObjects.AddToMoviesModelList(new MoviesModel(_movieInfo[i].Title, "(" + _movieInfo[i].ReleaseDate.Year.ToString() + ")",
                              string.Empty, path,
                             genreList, _movieInfo[i].Overview));
                            break;
                        case 1:
                            this._moviesObjects.AddToMoviesModelList(new MoviesModel(_movieInfo[i].Title, "(" + _movieInfo[i].ReleaseDate.Year.ToString() + ")",
                             movieInfoCastList[0].Name, path,
                            genreList, _movieInfo[i].Overview));
                            break;
                        case 2:
                            this._moviesObjects.AddToMoviesModelList(new MoviesModel(_movieInfo[i].Title, "(" + _movieInfo[i].ReleaseDate.Year.ToString() + ")",
                             movieInfoCastList[0].Name + ", " + movieInfoCastList[1].Name, path,
                            genreList, _movieInfo[i].Overview));
                            break;
                        default:
                            this._moviesObjects.AddToMoviesModelList(new MoviesModel(_movieInfo[i].Title, "(" + _movieInfo[i].ReleaseDate.Year.ToString() + ")",
                          movieInfoCastList[0].Name + ", " + movieInfoCastList[1].Name + ", " + movieInfoCastList[2].Name, path,
                         genreList, _movieInfo[i].Overview));
                            break;
                    }
                }

                
                await this.Navigation.PushAsync(new MovieListPage() { BindingContext = this._moviesObjects });
                this._movieEntry.Text = string.Empty;
                _ai.IsRunning = false;
                _searchMovieButton.IsEnabled = true;
            }
        }


    }
}
