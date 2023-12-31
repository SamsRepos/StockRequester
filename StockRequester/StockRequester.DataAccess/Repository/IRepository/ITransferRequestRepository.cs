using StockRequester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.DataAccess.Repository.IRepository
{
    public interface ITransferRequestRepository : IRepository<TransferRequest>
    {
        void Update(TransferRequest transferRequest);
    }
}
