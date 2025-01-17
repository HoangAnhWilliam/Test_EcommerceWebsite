﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWebsite.Models
{
    public class CategoryDetail
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name Required")]
        [StringLength(100, ErrorMessage = "Minimum 3, minimum 5 and maximum 100 charaters are allowed", MinimumLength = 3)]
        public string CategoryName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }

    public class ProductDetail
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Product name is required")]
        [StringLength(100, ErrorMessage = "Minimum 3, minimum 5 and maximum 100 charaters are allowed", MinimumLength = 3)]
        public string ProductName { get; set; }
        [Required]
        [Range(1,50)]
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Required(ErrorMessage =("Description is required"))]
        public Nullable<System.DateTime> Description { get; set; }
        public string ProductImage { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        [Required]
        [Range(typeof (int), "1", "500", ErrorMessage ="Invalid Quantity")]
        public Nullable<int> Quantity { get; set; }
        [Required]
        [Range(typeof(decimal), "1","200000", ErrorMessage ="Invalid Price")]
        public Nullable<decimal> Price { get; set; }
        public SelectList Categories { get; set; }
    }

    //public class MemeberDetail
    //{
    //    public int MemberId { get; set; }
    //    [Required(ErrorMessage = "Category Name Required")]
    //    [StringLength(100, ErrorMessage = "Minimum 3, minimum 5 and maximum 100 charaters are allowed", MinimumLength = 3)]
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string EmailId { get; set; }
    //    public string Password { get; set; }
    //    public Nullable<bool> IsActive { get; set; }
    //    public Nullable<bool> IsDelete { get; set; }
    //    public Nullable<System.DateTime> CreatedOn { get; set; }
    //    public Nullable<System.DateTime> ModifiedOn { get; set; }
    //}
}