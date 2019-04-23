using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VRWebServiceLibrary.Model;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerService.svc or CustomerService.svc.cs at the Solution Explorer and start debugging.
    public class CustomerService : ICustomerService
    {
        private  VideoRentalEntities db = new VideoRentalEntities();

        public IEnumerable<Customer> GetCustomers()
        {
            return db.Customers.ToList();
        }

        public Customer GetCustomer(int Id)
        {
            Customer customer = db.Customers.Find(Id);
            if(customer == null)
                return  new Customer();

            return customer;
        }

        public bool PutCustomer(int Id, Customer customer)
        {
            if (Id != customer.CustomerId)
                return false;

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public int PostCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.CustomerId;
        }

        public bool DeleteCustomer(int Id)
        {
            Customer customer = db.Customers.Find(Id);
            if (customer == null)
                return false;

            db.Customers.Remove(customer);
            db.SaveChanges();

            return true;
        }
    }
}
