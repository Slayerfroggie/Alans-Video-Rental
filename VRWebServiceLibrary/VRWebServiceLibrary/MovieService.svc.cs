using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VRWebServiceLibrary.Model;
using System.Web.Http.Results;
using System.Web.UI.WebControls;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MovieService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MovieService.svc or MovieService.svc.cs at the Solution Explorer and start debugging.
    [Serializable]
    public class MovieService : IMovieService
    {
        private  VideoRentalEntities db = new VideoRentalEntities();
       
        public IEnumerable<Movie> GetMovies()
        {
            //IEnumerable<Movie> movies = db.Movies.Cast<Movie>();
            //return movies;
            //var movies = (db.Movies.Select(m => m)).Select(m1 => new Movie()
            //{
            //    MovieId = m1.MovieId,
            //    Name = m1.Name
            //}).ToList();

            //return db.Movies.Cast<Movie>().ToList();
            return db.Movies.ToList();
        }


        public Movie GetMovie(int Id)
        {
            Movie movie = db.Movies.Find(Id);
            if (movie == null)
            {
                return new Movie();
            }

            return movie;
        }

        public bool PutMovie(int Id, Movie movie)
        {

            if (Id != movie.MovieId)
            {
                
                return false;
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Id))
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public int PostMovie(Movie movie)
        {
            db.Movies.Add(movie);
            db.SaveChanges();
            return movie.MovieId;
        }

        public bool DeleteMovie(int Id)
        {
            Movie movie = db.Movies.Find(Id);
            if (movie == null)
            {
                return false;
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return true;
        }

        private bool MovieExists(int Id)
        {
            return db.Movies.Count(e => e.MovieId == Id) > 0;
        }
    }
}
