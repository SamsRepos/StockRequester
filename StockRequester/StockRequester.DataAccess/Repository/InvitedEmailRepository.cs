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
    public class InvitedEmailRepository : Repository<InvitedEmail>, IInvitedEmailRepository
    {
        public InvitedEmailRepository(ApplicationDbContext db) 
            : 
            base(db)
        {
        }

    }
}
