using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_Ordering_Food.Models;
namespace Online_Ordering_Food.ViewModel
{
    public class ProductCart
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Cart> carts { get; set; }
    }
}