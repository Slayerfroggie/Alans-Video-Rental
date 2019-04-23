using System;
using System.Collections.Generic;
using VideoRental.Models;

namespace VideoRental.DAL
{
    public class VideoInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VideoContext>
    {
        protected override void Seed(VideoContext context)
        {
            var movies = new List<Movie>
            {
                new Movie{MovieId = 1, Name = "The Avengers"},
                new Movie{MovieId = 2, Name = "Star Wars"},
                new Movie{MovieId = 3, Name = "The Matrix"},

            };

            movies.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer{CustomerId = 1, CustomerName = "John Smith", Phone="3390 0675"},
                new Customer{CustomerId = 2, CustomerName = "Mary Parks", Phone="3855 1515"},
                new Customer{CustomerId = 3, CustomerName = "Robert Boyd", Phone="3290 9090"},

            };

            customers.ForEach(c => context.Customers.Add(c));
            context.SaveChanges();

            var rentals = new List<Rental>
            {
                new Rental{RentalId = 1, CustomerId = 1, DateRented = DateTime.Parse("01/01/2017"), DateReturned = null},
                new Rental{RentalId = 2, CustomerId = 2, DateRented = DateTime.Parse("01/01/2018"), DateReturned = null},
                new Rental{RentalId = 3, CustomerId = 3, DateRented = DateTime.Parse("01/05/2017"), DateReturned = null},
            };

            rentals.ForEach(r => context.Rentals.Add(r));
            context.SaveChanges();

            var rentalItems = new List<RentalItem>
            {
                new RentalItem{RentalItemId = 1, RentalId = 1, MovieId = 1},
                new RentalItem{RentalItemId = 2, RentalId = 1, MovieId = 2},
                new RentalItem{RentalItemId = 3, RentalId = 2, MovieId = 3},
                new RentalItem{RentalItemId = 4, RentalId = 3, MovieId = 1},
                new RentalItem{RentalItemId = 5, RentalId = 3, MovieId = 2},
                new RentalItem{RentalItemId = 6, RentalId = 3, MovieId = 3},
            };
            rentalItems.ForEach(ri => context.RentalItems.Add(ri));
            context.SaveChanges();
        }
    }
}