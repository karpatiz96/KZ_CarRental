using CarRental.Dal.Dtos;
using CarRental.Dal.Entities;
using CarRental.Dal.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CarRental.Dal.Filters.ReservationListFilter;

namespace CarRental.Dal.Services
{
    public class ReservationService : IReservationService
    {
        public CarRentalDbContext _dbContext { get; }

        private readonly UserManager<User> _userManager;

        public ReservationService(CarRentalDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public static Func<Reservation, ReservationHeader> ReservationHeaderSelector { get; } = r => new ReservationHeader
        {
            Id = r.Id,
            AddressId = r.AddressId,
            Address = r.Address.ZipCode + " " + r.Address.City + " " + r.Address.StreetAddress,
            CarId = r.CarId,
            Car = r.Car?.PlateNumber,
            DropOffTime = r.DropOffTime,
            PickUpTime = r.PickUpTime,
            Price = r.Price,
            State = r.State,
            UserId = r.UserId,
            User = r.User?.UserName,
            VehicleModelId = r.VehicleModelId,
            VehicleType = r.VehicleModel?.VehicleType
        };

        public static Func<Reservation, ReservationIndexHeader> ReservationIndexHeaderSelector { get; } = r => new ReservationIndexHeader
        {
            Id = r.Id,
            AddressId = r.AddressId,
            Address = r.Address.ZipCode + " " + r.Address.City + " " + r.Address.StreetAddress,
            CarId = r.CarId,
            Car = r.Car?.PlateNumber,
            DropOffTime = r.DropOffTime,
            PickUpTime = r.PickUpTime,
            State = r.State,
            VehicleModelId = r.VehicleModelId,
            VehicleType = r.VehicleModel?.VehicleType
        };

        public static Func<Reservation, ReservationListHeader> ReservationListHeaderSelector { get; } = r => new ReservationListHeader
        {
            Id = r.Id,
            AddressId = r.AddressId,
            Address = r.Address.ZipCode + " " + r.Address.City + " " + r.Address.StreetAddress,
            DropOffTime = r.DropOffTime,
            PickUpTime = r.PickUpTime,
            Price = r.Price,
            State = r.State,
            VehicleModelId = r.VehicleModelId,
            VehicleType = r.VehicleModel?.VehicleType
        };

        public async Task CreateReservation(Reservation reservation)
        {
            _dbContext.Add(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditReservation(int? id, int? carid)
        {
            var car = await _dbContext.Cars.Include(c => c.VehicleModel).Include(c => c.Reservations).Where(c => c.Id == carid).FirstOrDefaultAsync();
            var reservation = await _dbContext.Reservations.Where(r => r.Id == id).Include(r => r.Car).Include(r => r.Address).Include(r => r.User).Include(r => r.VehicleModel).FirstOrDefaultAsync();

            if(reservation.Car != null)
            {
                var oldCar = reservation.Car;
                oldCar.Reservations.Remove(reservation);
                reservation.Car = null;
                reservation.CarId = null;
                _dbContext.Attach(oldCar).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            reservation.CarId = car.Id;
            reservation.Car = car;
            reservation.State = Reservation.ReservationStates.Accepted;
            car.Reservations.Add(reservation);

            _dbContext.Attach(reservation).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelReservation(int? id)
        {
            var reservation = await _dbContext.Reservations.Where(r => r.Id == id).Include(r => r.Car).ThenInclude(c => c.Reservations).Include(r => r.Address).Include(r => r.User).Include(r => r.VehicleModel).FirstOrDefaultAsync();
            var car = reservation.Car;
            if(car != null)
            {
                car.Reservations.Remove(reservation);
                reservation.Car = null;
                reservation.CarId = null;
                _dbContext.Attach(car).State = EntityState.Modified;
            }
            reservation.State = Reservation.ReservationStates.Cancled;


            _dbContext.Attach(reservation).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReservation(int? id)
        {
            var reservation = await _dbContext.Reservations.Where(r => r.Id == id).Include(r => r.Car).ThenInclude(c => c.Reservations).Include(r => r.Address).ThenInclude(c => c.Reservations).Include(r => r.User).ThenInclude(c => c.Reservations).Include(r => r.VehicleModel).ThenInclude(c => c.Reservations).FirstOrDefaultAsync();

            var user = reservation.User;
            if (user != null)
            {
                user.Reservations.Remove(reservation);
                reservation.User = null;
                reservation.UserId = null;
            }
            var address = reservation.Address;
            if (address != null)
            {
                address.Reservations.Remove(reservation);
            }
            var car = reservation.Car;
            if (car != null)
            {
                car.Reservations.Remove(reservation);
                reservation.Car = null;
                reservation.CarId = null;
            }
            var vehiclemodel = reservation.VehicleModel;
            if (vehiclemodel != null)
            {
                vehiclemodel.Reservations.Remove(reservation);
                reservation.VehicleModel = null;
                reservation.VehicleModelId = null;
            }

            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<ReservationIndexHeader>> GetReservations(ReservationFilter filter)
        {
            if (filter == null)
            {
                filter = new ReservationFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<Reservation> reservations = _dbContext.Reservations.Include(c => c.VehicleModel).Include(c => c.Car).Include(c => c.Address).Include(c => c.User);

            if (!string.IsNullOrEmpty(filter?.VehicleType))
                reservations = reservations.Where(r => r.VehicleModel.VehicleType.Contains(filter.VehicleType));

            switch (filter.reservationOrder)
            {
                case ReservationFilter.ReservationOrder.AddressAscending:
                    reservations = reservations.OrderBy(r => r.Address.ZipCode).OrderBy(r => r.Address.City).OrderBy(r => r.Address.StreetAddress);
                    break;
                case ReservationFilter.ReservationOrder.AddressDescending:
                    reservations = reservations.OrderByDescending(r => r.Address.ZipCode).OrderByDescending(r => r.Address.City).OrderByDescending(r => r.Address.StreetAddress);
                    break;
                case ReservationFilter.ReservationOrder.PickUpAscending:
                    reservations = reservations.OrderBy(r => r.PickUpTime);
                    break;
                case ReservationFilter.ReservationOrder.PickUpDescending:
                    reservations = reservations.OrderByDescending(r => r.PickUpTime);
                    break;
                case ReservationFilter.ReservationOrder.DropOffAscending:
                    reservations = reservations.OrderBy(r => r.DropOffTime);
                    break;
                case ReservationFilter.ReservationOrder.DropOffDescending:
                    reservations = reservations.OrderByDescending(r => r.DropOffTime);
                    break;
                case ReservationFilter.ReservationOrder.VehicleModelAscending:
                    reservations = reservations.OrderBy(r => r.VehicleModel.VehicleType);
                    break;
                case ReservationFilter.ReservationOrder.VehicleModelDescending:
                    reservations = reservations.OrderByDescending(r => r.VehicleModel.VehicleType);
                    break;
                case ReservationFilter.ReservationOrder.StateAscending:
                    reservations = reservations.OrderBy(r => r.State);
                    break;
                case ReservationFilter.ReservationOrder.StateDescending:
                    reservations = reservations.OrderByDescending(r => r.State);
                    break;
                case ReservationFilter.ReservationOrder.CarAscending:
                    reservations = reservations.OrderBy(r => r.Car.PlateNumber);
                    break;
                case ReservationFilter.ReservationOrder.CarDescending:
                    reservations = reservations.OrderByDescending(r => r.Car.PlateNumber);
                    break;
                default:
                    break;
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = reservations.Count();
                reservations = reservations.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await reservations.ToListAsync();

            return new PagedResult<ReservationIndexHeader>
            {
                Total = Total,
                PageNumber = filter?.PageNumber,
                PageSize = filter?.PageSize,
                Results = results.Select(ReservationIndexHeaderSelector).ToList()
            };
        }

        public async Task<PagedResult<ReservationListHeader>> GetReservations(ReservationListFilter filter, int? userid)
        {
            if (filter == null)
            {
                filter = new ReservationListFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<Reservation> reservations = _dbContext.Reservations.Include(c => c.VehicleModel).Include(c => c.Car).Include(c => c.Address).Include(c => c.User);

            reservations = reservations.Where(r => r.UserId == userid);

            switch (filter.reservationOrder)
            {
                case ReservationOrder.AddressAscending:
                    reservations = reservations.OrderBy(r => r.Address.ZipCode).OrderBy(r => r.Address.City).OrderBy(r => r.Address.StreetAddress);
                    break;
                case ReservationOrder.AddressDescending:
                    reservations = reservations.OrderByDescending(r => r.Address.ZipCode).OrderByDescending(r => r.Address.City).OrderByDescending(r => r.Address.StreetAddress);
                    break;
                case ReservationOrder.PickUpAscending:
                    reservations = reservations.OrderBy(r => r.PickUpTime);
                    break;
                case ReservationOrder.PickUpDescending:
                    reservations = reservations.OrderByDescending(r => r.PickUpTime);
                    break;
                case ReservationOrder.DropOffAscending:
                    reservations = reservations.OrderBy(r => r.DropOffTime);
                    break;
                case ReservationOrder.DropOffDescending:
                    reservations = reservations.OrderByDescending(r => r.DropOffTime);
                    break;
                case ReservationOrder.VehicleModelAscending:
                    reservations = reservations.OrderBy(r => r.VehicleModel.VehicleType);
                    break;
                case ReservationOrder.VehicleModelDescending:
                    reservations = reservations.OrderByDescending(r => r.VehicleModel.VehicleType);
                    break;
                case ReservationOrder.StateAscending:
                    reservations = reservations.OrderBy(r => r.State);
                    break;
                case ReservationOrder.StateDescending:
                    reservations = reservations.OrderByDescending(r => r.State);
                    break;
                default:
                    break;
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = reservations.Count();
                reservations = reservations.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await reservations.ToListAsync();

            return new PagedResult<ReservationListHeader>
            {
                Total = Total,
                PageNumber = filter?.PageNumber,
                PageSize = filter?.PageSize,
                Results = results.Select(ReservationListHeaderSelector).ToList()
            };
        }

        public async Task<ReservationHeader> GetReservation(int? id)
        {
            var reservation = await _dbContext.Reservations
                .Include(r => r.VehicleModel)
                .Include(r => r.Car)
                .Include(r => r.Address)
                .Include(r => r.User).Where(r => r.Id == id.Value).FirstOrDefaultAsync();

            ReservationHeader reservationHeader = new ReservationHeader
            {
                Id = reservation.Id,
                AddressId = reservation.AddressId,
                Address = reservation.Address.ZipCode + " " + reservation.Address.City + " " + reservation.Address.StreetAddress,
                CarId = reservation.CarId,
                Car = reservation.Car?.PlateNumber,
                DropOffTime = reservation.DropOffTime,
                PickUpTime = reservation.PickUpTime,
                Price = reservation.Price,
                State = reservation.State,
                UserId = reservation.UserId,
                User = reservation.User?.UserName,
                VehicleModelId = reservation.VehicleModelId,
                VehicleType = reservation.VehicleModel?.VehicleType
            };

            return reservationHeader;
        }

        public IEnumerable<Reservation> GetReservations(int? userid)
        {
            return _dbContext.Reservations.Include(r => r.User).Where(r => r.UserId == userid).AsEnumerable().ToList();
        }

        public async Task DeletedUserReservations(int? userid)
        {
            var reservations = _dbContext.Reservations.Include(r => r.User).Include(r => r.Car).Where(r => r.UserId == userid).AsEnumerable().ToList();

            var user = await _dbContext.Users.Include(u => u.Reservations).Where(u => u.Id == userid).FirstOrDefaultAsync();

            foreach (var item in reservations)
            {
                item.User = null;
                user.Reservations.Remove(item);
                if (item.State == Reservation.ReservationStates.Undecieded)
                {
                    item.State = Reservation.ReservationStates.Cancled;
                }

                if(item.State == Reservation.ReservationStates.Accepted && item.PickUpTime.Date >= DateTime.Now.Date)
                {
                    var car = await _dbContext.Cars.Where(c => c.Id == item.CarId).Include(c => c.Reservations).FirstOrDefaultAsync();
                    if (car != null)
                    {
                        car.Reservations.Remove(item);
                        item.Car = null;
                        item.State = Reservation.ReservationStates.Cancled;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public bool ReservationExists(int? id)
        {
            return _dbContext.Reservations.Any(e => e.Id == id);
        }

    }
}
