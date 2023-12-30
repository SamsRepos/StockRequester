using StockRequester.DataAccess.Data;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext db) 
            : 
            base(db)
        {
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
