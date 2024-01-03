using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company {  get; }
        ILocationRepository Location { get; }
        ITransferRequestRepository TransferRequest { get; }
        IApplicationUserRepository ApplicationUser { get; }
        public void Save();
    }
}
