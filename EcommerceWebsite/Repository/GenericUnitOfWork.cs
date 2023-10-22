using EcommerceWebsite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWebsite.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private dbMyOnlineShoppingEntities dbEntity = new dbMyOnlineShoppingEntities();
        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
            return new GenericRepository<Tbl_EntityType>(dbEntity);
        } 

        public void SaveChanges()
        {
            dbEntity.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    dbEntity.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
    }
}