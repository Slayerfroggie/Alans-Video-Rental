using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VRWebServiceLibrary.Model;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RentalService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RentalService.svc or RentalService.svc.cs at the Solution Explorer and start debugging.
    public class RentalService : IRentalService
    {
        private VideoRentalEntities db = new VideoRentalEntities();

        public IEnumerable<Rental> GetRentals()
        {
            return db.Rentals.ToList();
        }

        public Rental GetRental(int Id)
        {
            Rental rental = db.Rentals.Find(Id);
            if(rental == null)
                return  new Rental();

            return rental;
        }

        public bool PutRental(int Id, Rental rental)
        {
            if (Id != rental.RentalId)
                return false;

            db.Entry(rental).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
            Console.WriteLine(e);
            throw;
            }
        }

        public int PostRental(Rental rental)
        {
            db.Rentals.Add(rental);
            db.SaveChanges();
            return rental.RentalId;
        }

        public bool DeleteRental(int Id)
        {
            Rental rental = db.Rentals.Find(Id);
            if (rental == null)
                return false;

            db.Rentals.Remove(rental);
            db.SaveChanges();

            return true;
        }
    }
}
