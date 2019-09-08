using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.SeedInterfaces
{
    public interface IUserSeedService
    {
        Task SeedUserAsync();
    }
}
