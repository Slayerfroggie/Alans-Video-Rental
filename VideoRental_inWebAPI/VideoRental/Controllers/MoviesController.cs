using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using VideoRental.DAL;
using VideoRental.Models;

namespace VideoRental.Controllers
{
    public class MoviesController : Controller
    {
        private VideoContext db = new VideoContext();

        // GET: Movies
        //public ActionResult Index()
        //{
        //    return View();
        //    //return Content("Hello World!");
        //    //return HttpNotFound();
        //    //return RedirectToAction("Index", "Home");
        //}

        // will return a list of movies in the database with 2 optional parameters
        // if page index is not specified, we display the movies in page 1 
        // and similarly if sortBy is not specified, we sort the movies by their name.
        // To make the parameter optional or nullable, we add a question mark after the data type as shown below.
        // The sortBy however is a string and string by default is null
        //[Route("movies/index/{pageIndex}/{sortBy}")]
        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue) pageIndex = 1;
        //    if (string.IsNullOrWhiteSpace(sortBy)) sortBy = "Name";

        //    // string format is the same as PageIndex = + pageIndex but better
        //    return Content(string.Format("PageIndex={0} and SortBy={1}", pageIndex, sortBy));
        //}

        public static List<Movie> movieList = new List<Movie>
        {
            new Movie{MovieId = 1, Name = "The Avengers"},
            new Movie{MovieId = 2, Name = "Star Wars"},
            new Movie{MovieId = 3, Name = "The Matrix"},

        };

        public ActionResult Index()
        {
            //var movies = from m in movieList
            //             orderby m.MovieId
            //             select m;

            //return View(movies);
            //return View(db.Movies.ToList());
            
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Movies").Result;
            // we are using IEnumerable because we only want to enumerate the collection and we are not going to add or delete elements
            IEnumerable<Movie> movies = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
            return View(movies);
        }

        public ActionResult Edit(int Id)
        {
            //var movie = movieList.Single(m => m.MovieId == Id);
            //return View(movie);
            //var movie = db.Movies.Single(m => m.MovieId == Id);
            //return View(movie);
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{Id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;
            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(int Id, Movie movie )
        {
            try {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Movies/{Id}", movie).Result;
                //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                TempData["SuccessMessage"] = "Saved successfully.";

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(movie);
            }
            catch {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{Id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;
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
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Movies", movie).Result;
                //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                TempData["SuccessMessage"] = "Movie added successfully.";

                return RedirectToAction("Index");
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
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{Id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;
            return View(movie);
        }

        [HttpPost]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Movies/{Id}").Result;
                //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                TempData["SuccessMessage"] = "Movie deleted successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}