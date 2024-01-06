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
    public class RequestStatusRepository : Repository<RequestStatus>, IRequestStatusRepository
    {
        public RequestStatusRepository(ApplicationDbContext db) 
            : 
            base(db)
        {
        }

        public override void Update(RequestStatus status)
        {
            _db.RequestStatuses.Update(status);
        }
    }
}
