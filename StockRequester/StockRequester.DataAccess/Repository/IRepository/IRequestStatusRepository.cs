using StockRequester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository.IRepository
{
    public interface IRequestStatusRepository : IRepository<RequestStatus>
    {
        void Update(RequestStatus status);
    }
}
