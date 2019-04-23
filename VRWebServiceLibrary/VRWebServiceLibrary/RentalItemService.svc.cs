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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RentalItemService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RentalItemService.svc or RentalItemService.svc.cs at the Solution Explorer and start debugging.
    public class RentalItemService : IRentalItemService
    {
        private VideoRentalEntities db = new VideoRentalEntities();

        public IEnumerable<RentalItem> GetRentalItems()
        {
            return db.RentalItems.ToList();
        }

        public RentalItem GetRentalItem(int Id)
        {
            RentalItem rentalItem = db.RentalItems.Find(Id);
            if(rentalItem == null)
                return  new RentalItem();

            return rentalItem;
        }

        public IEnumerable<RentalItem> GetRentalItemByRentalId(int Id)
        {
            return db.RentalItems.Where(ri => ri.RentalId == Id);
        }

        public bool PutRentalItem(int Id, RentalItem rentalItem)
        {
            if (Id != rentalItem.RentalItemId)
                return false;

            db.Entry(rentalItem).State = EntityState.Modified;

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

        public int PostRentalItem(RentalItem rentalItem)
        {
            db.RentalItems.Add(rentalItem);
            db.SaveChanges();
            return rentalItem.RentalItemId;
        }

        public int DeleteRentalItem(int Id)
        {
            RentalItem rentalItem = db.RentalItems.Find(Id);
            if (rentalItem == null)
                return 0;

            db.RentalItems.Remove(rentalItem);
            db.SaveChanges();

            return rentalItem.RentalId;
        }
    }
}
