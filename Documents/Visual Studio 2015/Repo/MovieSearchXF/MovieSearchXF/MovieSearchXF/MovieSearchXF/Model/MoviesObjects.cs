﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchXF.Model
{
    public class MoviesObjects
    {

        public List<MoviesModel> _moviesModelList = new List<MoviesModel>();

        public List<MoviesModel> MoviesModel => this._moviesModelList;

        //public MoviesObjects(MoviesModel moviesModel)
        //{
        //    this._moviesModelList.Add(moviesModel);
        //}
        public void AddToMoviesModelList(MoviesModel moviesModel)
        {
            this._moviesModelList.Add(moviesModel);
        }

        public List<MoviesModel>  returnListOfMoviesModel()
        {
            return this._moviesModelList;
        }
        public MoviesObjects()
        {
            
        }
    }
}
