using StockRequester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<Company> Company {  get; }
        IRepository<Location> Location { get; }
        IRepository<TransferRequest> TransferRequest { get; }
        IRepository<ApplicationUser> ApplicationUser { get; }
        IRepository<InvitedEmail> InvitedEmail { get; }
        IRepository<RequestStatus> RequestStatus { get; }
        public void Save();
    }
}
