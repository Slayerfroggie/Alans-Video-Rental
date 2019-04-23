using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using VideoRental.DAL;
using VideoRental.MovieService;
using Movie = VideoRental.Models.Movie;

namespace VideoRental.Controllers
{
    public class MoviesController : Controller
    {
        MovieService.MovieServiceClient _movieService = new MovieService.MovieServiceClient();

       public ActionResult Index()
        {
            IEnumerable<Movie> movies = _movieService.GetMovies().ToList().Select(m => new Movie()
            {
                MovieId = m.MovieId,
                Name = m.Name
            });

            return View(movies.ToList());
        }

        public ActionResult Edit(int Id)
        {
            var movieSvc = _movieService.GetMovie(Id);
            Movie movie = new Movie()
            {
                MovieId = movieSvc.MovieId,
                Name = movieSvc.Name
            };


            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(int Id, Movie movie )
        {
            try
            {
                MovieService.Movie movieSvc = new MovieService.Movie()
                {
                    MovieId = movie.MovieId,
                    Name = movie.Name
                };

                bool IsSuccess = _movieService.PutMovie(Id, movieSvc);

                if (IsSuccess)
                {
                     //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                     TempData["SuccessMessage"] = "Saved successfully.";
                    return RedirectToAction("Index");
                }

                return View(movie);
            }
            catch {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            Movie movie = new Movie()
            {
                MovieId = _movieService.GetMovie(Id).MovieId,
                Name = _movieService.GetMovie(Id).Name
            };

            return View(movie);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            try {

                MovieService.Movie movieSvc = new MovieService.Movie()
                {
                    MovieId = movie.MovieId,
                    Name = movie.Name
                };

                int Id = _movieService.PostMovie(movieSvc); 
                if (Id > 0)
                {
                    //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                    TempData["SuccessMessage"] = "Movie added successfully.";

                    return RedirectToAction("Index");
                }

                return View(movie);
            }
            catch {
                return View();
            }
        }


        public ActionResult DisplayVideo()
        {
            var movie = new Movie() { Name = "The Avengers" };

            return View(movie);
        }

        public ActionResult Delete(int Id)
        {
            Movie movie = new Movie()
            {
                MovieId = _movieService.GetMovie(Id).MovieId,
                Name = _movieService.GetMovie(Id).Name
            };

            return View(movie);
        }

        [HttpPost]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            try
            {
                if (_movieService.DeleteMovie(Id))
                {
                    //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                    TempData["SuccessMessage"] = "Movie deleted successfully.";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

    }
}