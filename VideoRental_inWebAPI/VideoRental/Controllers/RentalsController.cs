using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using VideoRental.DAL;
using VideoRental.Models;
using VideoRental.ViewModels;

namespace VideoRental.Controllers
{
    public class RentalsController : Controller
    {
        private VideoContext db = new VideoContext();

        // GET: Rentals
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Rentals").Result;
            IEnumerable<Rental> rentals = response.Content.ReadAsAsync<IEnumerable<Rental>>().Result;
            response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customers = response.Content.ReadAsAsync<IList<Customer>>().Result;

            var customerRentalsViewModel = rentals.Select(
                r => new CustomerRentalsViewModel
                {
                    RentalId = r.RentalId,
                    DateRented = r.DateRented,
                    CustomerName = customers.Where(c => c.CustomerId == r.CustomerId).Select(u => u.CustomerName).FirstOrDefault()
                }).OrderByDescending(o => o.DateRented).ToList();

            return View(customerRentalsViewModel);
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
                var rental = response.Content.ReadAsAsync<Rental>().Result;
                response = WebClient.ApiClient.GetAsync($"RentalItemsById/{Id}").Result;
                IList<RentalItem> rentalItems = response.Content.ReadAsAsync<IList<RentalItem>>().Result;
                response = WebClient.ApiClient.GetAsync("Movies").Result;
                IList<Movie> dbMovies = response.Content.ReadAsAsync<IList<Movie>>().Result;

                var customers = GetCustomers();
                var rentedMovies = rentalItems.Select(
                        m => new CustomerMoviesViewModel
                        {
                            RentalItemId = m.RentalItemId,
                            RentalId = m.RentalId,
                            MovieName = dbMovies.Where(c => c.MovieId == m.MovieId).Select(f => f.Name).FirstOrDefault()
                        }).ToList();

                rental.Customers = customers;
                rental.RentedMovies = rentedMovies;

                return View(rental);
            }
            catch {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, Rental rental)
        {
            try {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Rentals/{Id}", rental).Result;
                
                if(response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                
                return View(rental);
            }
            catch {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;
            response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customers = response.Content.ReadAsAsync<IList<Customer>>().Result;
            response = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> dbMovies = response.Content.ReadAsAsync<IList<Movie>>().Result;

            var customerRentalDetails = new CustomerRentalDetailsViewModel
                {
                    Rental = rental,
                    CustomerName = customers.Select(cu => cu.CustomerName).FirstOrDefault(),
                    RentedMovies = rental.RentalItems.Select(
                        ri => new CustomerMoviesViewModel
                        {
                            RentalId = ri.RentalId,
                            MovieName = dbMovies.Where(c2 => c2.MovieId == ri.MovieId).Select(m => m.Name).FirstOrDefault()
                        }).ToList()
                };

            return View(customerRentalDetails);
        }

        public ActionResult Create()
        {
            var rental = new Rental();
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("GetRentalMaxId").Result;
            // Setting the primary key value to a negative value will make SQL server to find the next available PKID when you save it.
            rental.RentalId = -999;
            rental.DateRented = DateTime.Now;
            var customers = GetCustomers();
            rental.Customers = customers;
            rental.RentedMovies = new List<CustomerMoviesViewModel>();

            return View(rental);
        }

        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            try {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Rentals", rental).Result;
                rental = response.Content.ReadAsAsync<Rental>().Result;
                response = WebClient.ApiClient.GetAsync($"RentalItemsById/{rental.RentalId}").Result;
                IList<RentalItem> rentalItems = response.Content.ReadAsAsync<IList<RentalItem>>().Result;

                if (rentalItems.Count == 0)
                    return RedirectToAction("Edit", new { Id = rental.RentalId });
                else
                    return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        public ActionResult Delete(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;

            return View(rental);
        }

        [HttpPost]
        public ActionResult Delete(int Id, Rental rental)
        {
            try {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Rentals/{Id}").Result;

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        public ActionResult AddMovies(int RentalId)
        {
            var rentalItem = new RentalItem();
            var movies = GetMovies();
            rentalItem.RentalId = RentalId;
            rentalItem.Movies = movies;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult AddMovies(RentalItem rentalItem)
        {
            int Id = 0;
            try {
                Id = rentalItem.RentalId;
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("RentalItems", rentalItem).Result;

                return RedirectToAction("Edit", new { Id });
            }
            catch (Exception) {
                return View("No record of the associated rental can be found.  Make sure to submit the rental details before adding movies.");
            }
        }

        public ActionResult EditRentedMovie(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            var movies = GetMovies();
            rentalItem.Movies = movies;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult EditRentedMovie(int Id, RentalItem rentalItem)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"RentalItems/{Id}", rentalItem).Result;
                Id = rentalItem.RentalId;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Edit", new { Id });

                return View(rentalItem);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteRentedMovie(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            var movies = GetMovies();
            rentalItem.Movies = movies;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult DeleteRentedMovie(int Id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"RentalItems/{Id}").Result;
                var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
                Id = rentalItem.RentalId;
                return RedirectToAction("Edit", new { Id });
            }
            catch
            {
                return View();
            }
        }

        public IEnumerable<SelectListItem> GetMovies()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> dbMovies = response.Content.ReadAsAsync<IList<Movie>>().Result;

            List<SelectListItem> movies = dbMovies
                                            .OrderBy(o => o.Name)
                                            .Select(m => new SelectListItem
                                            {
                                                Value = m.MovieId.ToString(),
                                                Text = m.Name
                                            }).ToList();

            return new SelectList(movies, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetCustomers()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> dbCustomers = response.Content.ReadAsAsync<IList<Customer>>().Result;
            List<SelectListItem> customers = dbCustomers
                .OrderBy(o => o.CustomerName)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName
                }).ToList();

            return new SelectList(customers, "Value", "Text");
        }

    }
}