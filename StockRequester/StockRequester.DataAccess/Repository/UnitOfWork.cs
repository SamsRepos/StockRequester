using StockRequester.DataAccess.Data;
using StockRequester.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICompanyRepository Company {  get; private set; }
        public ILocationRepository Location { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Company = new CompanyRepository(_db);
            Location = new LocationRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
