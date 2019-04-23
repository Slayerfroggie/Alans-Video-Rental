using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoRentalWebAPI.ViewModels
{
    public class CustomerMoviesViewModel
    {
        public int RentalId { get; set; }
        public int RentalItemId { get; set; }
        public string MovieName { get; set; }
    }
}