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
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IRepository<Company> Company { get; private set; }
        public IRepository<Location> Location { get; private set; }
        public IRepository<TransferRequest> TransferRequest { get; private set; }
        public IRepository<ApplicationUser> ApplicationUser { get; private set; }
        public IRepository<InvitedEmail> InvitedEmail { get; private set; }
        public IRepository<RequestStatus> RequestStatus { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Company         = new Repository<Company>(_db);
            Location        = new Repository<Location>(_db);
            TransferRequest = new Repository<TransferRequest>(_db);
            ApplicationUser = new Repository<ApplicationUser>(_db);
            InvitedEmail    = new Repository<InvitedEmail>(_db);
            RequestStatus   = new Repository<RequestStatus>(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
