using EcommerceWebsite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWebsite.Models.Home
{
    public class Item
    {
        public Tbl_Product product {  get; set; }
        public int quantity { get; set; }

    }
}