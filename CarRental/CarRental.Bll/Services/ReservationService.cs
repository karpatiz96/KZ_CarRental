﻿using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CarRental.Bll.Filters.ReservationListFilter;

namespace CarRental.Bll.Services
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

        public static Expression<Func<Reservation, ReservationIndexHeader>> ReservationIndexHeaderSelector { get; } = r => new ReservationIndexHeader
        {
            Id = r.Id,
            AddressId = r.AddressId,
            Address = r.Address.ZipCode + " " + r.Address.City + " " + r.Address.StreetAddress,
            CarId = r.CarId,
            Car = r.Car.PlateNumber ?? "",
            DropOffTime = r.DropOffTime,
            PickUpTime = r.PickUpTime,
            State = r.State,
            VehicleModelId = r.VehicleModelId,
            VehicleType = r.VehicleModel.VehicleType ?? ""
        };

        public static Expression<Func<Reservation, ReservationListHeader>> ReservationListHeaderSelector { get; } = r => new ReservationListHeader
        {
            Id = r.Id,
            AddressId = r.AddressId,
            Address = r.Address.ZipCode + " " + r.Address.City + " " + r.Address.StreetAddress,
            DropOffTime = r.DropOffTime,
            PickUpTime = r.PickUpTime,
            Price = r.Price,
            State = r.State,
            VehicleModelId = r.VehicleModelId.Value,
            VehicleType = r.VehicleModel.VehicleType ?? ""
        };

        public async Task CreateReservation(ReservationDto reservationDto)
        {
            var reservation = new Reservation
            {
                UserId = reservationDto.UserId,
                AddressId = reservationDto.AddressId,
                VehicleModelId = reservationDto.VehicleModelId,
                PickUpTime = reservationDto.PickUpTime,
                DropOffTime = reservationDto.DropOffTime,
                Price = reservationDto.Price,
                State = Reservation.ReservationStates.Undecieded
            };

            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditReservation(int? id, int? carid)
        {
            var car = await _dbContext.Cars
                .Include(c => c.Reservations)
                .Where(c => c.Id == carid)
                .SingleOrDefaultAsync();

            var reservation = await _dbContext.Reservations
                .Where(r => r.Id == id)
                .Include(r => r.Car)
                .SingleOrDefaultAsync();

            if (reservation.Car != null)
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
            var reservation = await _dbContext.Reservations
                .Where(r => r.Id == id)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Reservations)
                .SingleOrDefaultAsync();

            var car = reservation.Car;
            if (car != null)
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
            var reservation = await _dbContext.Reservations
                .Where(r => r.Id == id)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Reservations)
                .Include(r => r.Address)
                    .ThenInclude(c => c.Reservations)
                .Include(r => r.User)
                    .ThenInclude(c => c.Reservations)
                .Include(r => r.VehicleModel)
                    .ThenInclude(c => c.Reservations)
                .SingleOrDefaultAsync();

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

            IQueryable<Reservation> reservations = _dbContext.Reservations
                .Include(c => c.VehicleModel)
                .Include(c => c.Car)
                .Include(c => c.Address)
                .Include(c => c.User);

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

            var results = await reservations.Select(ReservationIndexHeaderSelector).ToListAsync();

            return new PagedResult<ReservationIndexHeader>
            {
                Total = Total,
                PageNumber = filter?.PageNumber,
                PageSize = filter?.PageSize,
                Results = results
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

            IQueryable<Reservation> reservations = _dbContext.Reservations
                .Include(c => c.VehicleModel)
                .Include(c => c.Car)
                .Include(c => c.Address)
                .Include(c => c.User);

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
                case ReservationOrder.PriceAscending:
                    reservations = reservations.OrderBy(r => r.Price);
                    break;
                case ReservationOrder.PriceDescending:
                    reservations = reservations.OrderByDescending(r => r.Price);
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

            var results = await reservations.Select(ReservationListHeaderSelector).ToListAsync();

            return new PagedResult<ReservationListHeader>
            {
                Total = Total,
                PageNumber = filter?.PageNumber,
                PageSize = filter?.PageSize,
                Results = results
            };
        }

        public async Task<ReservationHeader> GetReservation(int? id)
        {
            var reservation = await _dbContext.Reservations
                .Include(r => r.VehicleModel)
                .Include(r => r.Car)
                .Include(r => r.Address)
                .Include(r => r.User)
                .Where(r => r.Id == id.Value)
                .SingleOrDefaultAsync();

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

        public async Task DeletedUserReservations(int? userid)
        {
            var reservations = _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.UserId == userid)
                .AsEnumerable()
                .ToList();

            var user = await _dbContext.Users
                .Include(u => u.Reservations)
                .Where(u => u.Id == userid)
                .SingleOrDefaultAsync();

            foreach (var item in reservations)
            {
                item.User = null;
                user.Reservations.Remove(item);
                if (item.State == Reservation.ReservationStates.Undecieded)
                {
                    item.State = Reservation.ReservationStates.Cancled;
                }

                if (item.State == Reservation.ReservationStates.Accepted && item.PickUpTime.Date >= DateTime.Now.Date)
                {
                    var car = await _dbContext.Cars.Where(c => c.Id == item.CarId).Include(c => c.Reservations).SingleOrDefaultAsync();
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
            return _dbContext.Reservations
                .Any(e => e.Id == id);
        }

        public async Task<IEnumerable<ReservationListHeader>> GetReservationListHeaders(int? userid)
        {
            var reservations = await _dbContext.Reservations
                .Where(r => r.UserId == userid.Value)
                .Where(r => r.DropOffTime > DateTime.Now)
                .Select(ReservationListHeaderSelector)
                .ToListAsync();

            return reservations;
        }

    }
}
