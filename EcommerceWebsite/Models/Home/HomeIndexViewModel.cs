using EcommerceWebsite.DAL;
using EcommerceWebsite.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace EcommerceWebsite.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbMyOnlineShoppingEntities context =new dbMyOnlineShoppingEntities();
        public IPagedList<Tbl_Product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize , int? page)
        {
            //var viewModel = new HomeIndexViewModel();
            //viewModel.ListOfProducts = viewModel._unitOfWork.GetRepositoryInstance<Tbl_Product>().GetAllRecords();
            //return viewModel;
            SqlParameter[] param = new SqlParameter[] { 
                new SqlParameter("@search", search??(object)DBNull.Value)
            };
            IPagedList<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {
                ListOfProducts  = data
            };
        }
    }
}