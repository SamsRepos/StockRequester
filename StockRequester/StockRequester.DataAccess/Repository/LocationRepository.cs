using StockRequester.DataAccess.Data;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;

namespace StockRequester.DataAccess.Repository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext db) 
            : 
            base(db)
        {
        }

        public void Update(Location location)
        {
            //_db.Locations.Update(location);

            var objFromDb = _db.Locations.FirstOrDefault(u => u.Id == location.Id);
            if (objFromDb is not null)
            {
                objFromDb.Name = location.Name;
                objFromDb.CompanyId = location.CompanyId;
            }
        }
    }
}
