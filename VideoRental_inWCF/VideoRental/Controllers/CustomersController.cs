using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using VideoRental.DAL;
using VideoRental.Models;
using VideoRental.CustomerService;
using Customer = VideoRental.Models.Customer;

namespace VideoRental.Controllers
{
    public class CustomersController : Controller
    {
        CustomerServiceClient _customerService = new CustomerServiceClient();

        // GET: Customers
        public ActionResult Index()
        {
            IEnumerable<Customer> customers = _customerService.GetCustomers().ToList().Select(c => new Customer()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            });

            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int Id)
        {
            var customerSvc = _customerService.GetCustomer(Id);
            Customer customer = new Customer()
            {
                CustomerId = customerSvc.CustomerId,
                CustomerName = customerSvc.CustomerName,
                Phone = customerSvc.Phone
            };

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                CustomerService.Customer customerSvc = new CustomerService.Customer()
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Phone = customer.Phone
                };

                int Id = _customerService.PostCustomer(customerSvc);
                if (Id > 0)
                {
                    //we will refer to this in the Index.cshtml of the Movie so alertify can display the message.
                    TempData["SuccessMessage"] = "Movie added successfully.";

                    return RedirectToAction("Index");
                }

                return View(customer);
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int Id)
        {
            var customerSvc = _customerService.GetCustomer(Id);
            Customer customer = new Customer()
            {
                CustomerId = customerSvc.CustomerId,
                CustomerName = customerSvc.CustomerName,
                Phone = customerSvc.Phone
            };

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(int Id, Customer customer)
        {
            try
            {
                CustomerService.Customer customerSvc = new CustomerService.Customer()
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Phone = customer.Phone
                };

                bool IsSuccess = _customerService.PutCustomer(Id, customerSvc);

                if (IsSuccess)
                    return RedirectToAction("Index");

                return View(customer);
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int Id)
        {
            var customerSvc = _customerService.GetCustomer(Id);
            Customer customer = new Customer()
            {
                CustomerId = customerSvc.CustomerId,
                CustomerName = customerSvc.CustomerName,
                Phone = customerSvc.Phone
            };

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(int Id, Customer customer)
        {
            try
            {
                if (_customerService.DeleteCustomer(Id))
                {
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
