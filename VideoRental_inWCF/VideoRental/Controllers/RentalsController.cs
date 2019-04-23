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
using VideoRental.RentalService;
using VideoRental.RentalItemService;
using VideoRental.CustomerService;
using VideoRental.MovieService;
using Customer = VideoRental.Models.Customer;
using Movie = VideoRental.Models.Movie;
using Rental = VideoRental.Models.Rental;
using RentalItem = VideoRental.Models.RentalItem;


namespace VideoRental.Controllers
{
    public class RentalsController : Controller
    {
        RentalServiceClient _rentalService = new RentalServiceClient();
        RentalItemServiceClient _rentalItemService = new RentalItemServiceClient();
        CustomerServiceClient _customerService = new CustomerServiceClient();
        MovieServiceClient _movieService = new MovieServiceClient();

        // GET: Rentals
        public ActionResult Index()
        {
            IEnumerable<Rental> rentals = _rentalService.GetRentals().ToList().Select(r => new Rental()
            {
                RentalId = r.RentalId,
                CustomerId = r.CustomerId,
                DateRented = r.DateRented,
                DateReturned = r.DateReturned
            });

            IEnumerable<Customer> customers = _customerService.GetCustomers().ToList().Select(c => new Customer()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            });

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
                var rentalSvc = _rentalService.GetRental(Id);
                Rental rental = new Rental()
                {
                    RentalId = rentalSvc.RentalId,
                    CustomerId = rentalSvc.CustomerId,
                    DateRented = rentalSvc.DateRented,
                    DateReturned = rentalSvc.DateReturned
                };
                
                
                IEnumerable<RentalItem> rentalItems = _rentalItemService.GetRentalItemByRentalId(Id).ToList().Select(ri => new RentalItem()
                {
                    RentalItemId = ri.RentalItemId,
                    RentalId = ri.RentalId,
                    MovieId = ri.MovieId
                });


                IEnumerable<Movie> movies = _movieService.GetMovies().ToList().Select(m => new Movie()
                {
                    MovieId = m.MovieId,
                    Name = m.Name
                });

                var customers = GetCustomers();

                var rentedMovies = rentalItems.Select(
                        m => new CustomerMoviesViewModel
                        {
                            RentalItemId = m.RentalItemId,
                            RentalId = m.RentalId,
                            MovieName = movies.Where(c => c.MovieId == m.MovieId).Select(f => f.Name).FirstOrDefault()
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
                RentalService.Rental rentalSvc = new RentalService.Rental()
                {
                    RentalId = rental.RentalId,
                    CustomerId = rental.CustomerId,
                    DateRented = rental.DateRented,
                    DateReturned = rental.DateReturned
                };

                bool IsSuccess = _rentalService.PutRental(Id, rentalSvc);
                if(IsSuccess)
                    return RedirectToAction("Index");
                
                return View(rental);
            }
            catch {
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            var rentalSvc = _rentalService.GetRental(Id);
            IEnumerable<RentalItem> rentalItems = _rentalItemService.GetRentalItemByRentalId(Id).ToList().Select(ri => new RentalItem()
            {
                RentalItemId = ri.RentalItemId,
                RentalId = ri.RentalId,
                MovieId = ri.MovieId
            });

            Rental rental = new Rental()
            {
                RentalId = rentalSvc.RentalId,
                CustomerId = rentalSvc.CustomerId,
                DateRented = rentalSvc.DateRented,
                DateReturned = rentalSvc.DateReturned,
                RentalItems = rentalItems.Select(ri => new RentalItem()
                {
                    RentalItemId = ri.RentalItemId,
                    RentalId = ri.RentalId,
                    MovieId = ri.MovieId
                }).ToList()
            };

            IList<Customer> customers = _customerService.GetCustomers().Select(c => new Customer()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            }).ToList();

            IEnumerable<Movie> movies = _movieService.GetMovies().ToList().Select(m => new Movie()
            {
                MovieId = m.MovieId,
                Name = m.Name
            });

            var customerRentalDetails = new CustomerRentalDetailsViewModel
                {
                    Rental = rental,
                    CustomerName = customers.Select(cu => cu.CustomerName).FirstOrDefault(),
                    RentedMovies = rental.RentalItems.Select(
                        ri => new CustomerMoviesViewModel
                        {
                            RentalId = ri.RentalId,
                            MovieName = movies.Where(c2 => c2.MovieId == ri.MovieId).Select(m => m.Name).FirstOrDefault()
                        }).ToList()
                };

            return View(customerRentalDetails);
        }

        public ActionResult Create()
        {
            var rental = new Rental
            {
                // Setting the primary key value to a negative value will make SQL server to find the next available PKID when you save it.
                RentalId = -999,
                DateRented = DateTime.Now
            };
            var customers = GetCustomers();
            rental.Customers = customers;
            rental.RentedMovies = new List<CustomerMoviesViewModel>();

            return View(rental);
        }

        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            try {
                RentalService.Rental rentalSvc = new RentalService.Rental()
                {
                    RentalId = rental.RentalId,
                    CustomerId = rental.CustomerId,
                    DateRented = rental.DateRented,
                    DateReturned = rental.DateReturned
                };

                int Id = _rentalService.PostRental(rentalSvc);

                IEnumerable<RentalItem> rentalItems = _rentalItemService.GetRentalItemByRentalId(Id).ToList().Select(ri => new RentalItem()
                {
                    RentalItemId = ri.RentalItemId,
                    RentalId = ri.RentalId,
                    MovieId = ri.MovieId
                });

                if (rentalItems.Any())
                    return RedirectToAction("Edit", new { Id = Id });
                else
                    return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        public ActionResult Delete(int Id)
        {
            var rentalSvc = _rentalService.GetRental(Id);
            Rental rental = new Rental()
            {
                RentalId = rentalSvc.RentalId,
                CustomerId = rentalSvc.CustomerId,
                DateRented = rentalSvc.DateRented,
                DateReturned = rentalSvc.DateReturned
            };


            return View(rental);
        }

        [HttpPost]
        public ActionResult Delete(int Id, Rental rental)
        {
            try
            {
                if(_rentalService.DeleteRental(Id))
                    return RedirectToAction("Index");

                return View();
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
            try
            {
                Id = rentalItem.RentalId;

                RentalItemService.RentalItem rentalItemSvc = new RentalItemService.RentalItem()
                {
                    RentalItemId = rentalItem.RentalItemId,
                    RentalId = rentalItem.RentalId,
                    MovieId = rentalItem.MovieId
                };

                _rentalItemService.PostRentalItem(rentalItemSvc);

                return RedirectToAction("Edit", new { Id });
            }
            catch (Exception) {
                return View("No record of the associated rental can be found.  Make sure to submit the rental details before adding movies.");
            }
        }

        public ActionResult EditRentedMovie(int Id)
        {
            var rentalItemSvc = _rentalItemService.GetRentalItem(Id);
            RentalItem rentalItem = new RentalItem()
            {
                RentalItemId = rentalItemSvc.RentalItemId,
                RentalId = rentalItemSvc.RentalId,
                MovieId = rentalItemSvc.MovieId
            };

            var movies = GetMovies();
            rentalItem.Movies = movies;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult EditRentedMovie(int Id, RentalItem rentalItem)
        {
            try
            {
                RentalItemService.RentalItem rentalItemSvc = new RentalItemService.RentalItem()
                {
                    RentalItemId = rentalItem.RentalItemId,
                    RentalId = rentalItem.RentalId,
                    MovieId = rentalItem.MovieId
                };

                bool IsSuccess = _rentalItemService.PutRentalItem(Id, rentalItemSvc);

                Id = rentalItem.RentalId;
                if (IsSuccess)
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
            var rentalItemSvc = _rentalItemService.GetRentalItem(Id);
            RentalItem rentalItem = new RentalItem()
            {
                RentalItemId = rentalItemSvc.RentalItemId,
                RentalId = rentalItemSvc.RentalId,
                MovieId = rentalItemSvc.MovieId
            };

            var movies = GetMovies();
            rentalItem.Movies = movies;

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult DeleteRentedMovie(int Id, RentalItem rentalItem)
        {
            try
            {
                int RentalId = _rentalItemService.DeleteRentalItem(Id);
                if (RentalId > 0)
                {
                    return RedirectToAction("Edit", new { Id = RentalId });
                }

                return View();

            }
            catch
            {
                return View();
            }
        }

        public IEnumerable<SelectListItem> GetMovies()
        {
            IEnumerable<Movie> dbMovies = _movieService.GetMovies().ToList().Select(m => new Movie()
            {
                MovieId = m.MovieId,
                Name = m.Name
            });


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
            IEnumerable<Customer> dbCustomers = _customerService.GetCustomers().ToList().Select(c => new Customer()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            });

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