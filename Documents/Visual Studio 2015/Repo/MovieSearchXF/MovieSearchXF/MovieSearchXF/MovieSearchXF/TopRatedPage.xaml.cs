using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieSearchXF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MovieSearchXF
{
    public partial class TopRatedPage : ContentPage
    {
        private ApiSearchResponse<MovieInfo> _responseMovieInfo;
        private IReadOnlyList<MovieInfo> _movieInfo;
        private IApiMovieRequest _movieApi;
        public MoviesObjects _moviesObjects;
        private ActivityIndicator _ai;
        private ListView _lv;

        public TopRatedPage()
        {
            InitializeComponent();
            this._ai = this.FindByName<ActivityIndicator>("ai");
            _ai.IsVisible = false;
            _lv = this.FindByName<ListView>("listview");
            this._moviesObjects = new MoviesObjects();
            MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            TopRated();

            _lv.RefreshCommand = new Command(() =>
            {
                TopRated();
            });

        }

        private void Listview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            this.Navigation.PushAsync(new MovieExtendedInfoPage() { BindingContext = e.SelectedItem });
        }

        public async Task TopRated()
        {
            _ai.IsRunning = true;
            _ai.IsVisible = true;

            this._moviesObjects._moviesModelList.Clear();

            _responseMovieInfo = await _movieApi.GetTopRatedAsync();
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
                    path = "http://image.tmdb.org/t/p/w92" + _movieInfo[i].PosterPath;
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

            this.BindingContext = this._moviesObjects;
            _ai.IsRunning = false;
            _ai.IsVisible = false;
            _lv.EndRefresh();

        }

      
         
        }
}
