using System.Threading.Tasks;
using Vega.Core;

namespace Vega.Core
{
    public interface IVehicleRepository
    {
        void Add(Vehicle vehicle);  
        Task<Vehicle> GetVehicles(int id, bool includeRelated = true);       
        void Remove(Vehicle vehicle);
    }
}