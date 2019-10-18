using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        public CarRentalDbContext _dbContext { get; }

        private readonly IHostingEnvironment _hosting;

        public VehicleModelService(CarRentalDbContext dbContext, IHostingEnvironment hosting)
        {
            _dbContext = dbContext;
            _hosting = hosting;
        }

        public static Expression<Func<VehicleModel, VehicleDto>> VehicleDtoSelector { get; } = vm => new VehicleDto
        {
            Id = vm.Id,
            PricePerDay = vm.PricePerDay,
            VehicleType = vm.VehicleType,
            VehicleUrl = vm.VehicleUrl
        };

        public static Expression<Func<VehicleModel, VehicleModelDto>> VehicleModelDtoSelector { get; } = vm => new VehicleModelDto
        {
            Id = vm.Id,
            VehicleType = vm.VehicleType,
            PricePerDay = vm.PricePerDay,
            VehicleUrl = vm.VehicleUrl,
            NumberOfDoors = vm.NumberOfDoors,
            NumberOfSeats = vm.NumberOfSeats,
            Automatic = vm.Automatic,
            AirConditioning = vm.AirConditioning,
            Active = vm.Active
        };

        public static Expression<Func<VehicleModel, VehicleModelDetailsDto>> VehicleModelDetailsDtoSelector { get; } = vm => new VehicleModelDetailsDto
        {
            Id = vm.Id,
            VehicleType = vm.VehicleType,
            PricePerDay = vm.PricePerDay,
            VehicleUrl = vm.VehicleUrl,
            NumberOfDoors = vm.NumberOfDoors,
            NumberOfSeats = vm.NumberOfSeats,
            Automatic = vm.Automatic,
            AirConditioning = vm.AirConditioning,
            Active = vm.Active,
            Cars = vm.Cars.Select(c => new CarDto
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                VehicleModelId = c.VehicleModelId,
                VehicleType = c.VehicleModel.VehicleType,
                Active = c.Active
            }).ToList(),
            CarFound = vm.Cars.Count,
            StarRating = vm.Ratings.Any() ? (vm.Ratings.Sum(r => (float) r.Value) / (float) vm.Ratings.Count()) : 0,
            Reviewers = vm.Ratings.Count()
        };

        public async Task<IEnumerable<VehicleModel>> GetVehicleModels()
        {
            return await _dbContext.VehicleModels
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleModel>> GetActiveVehicleModels()
        {
            return await _dbContext.VehicleModels
                .Where(vm => vm.Active == true)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleDto>> GetBestOffers(int size)
        {
            var vehicles = await _dbContext.VehicleModels
                .Where(vm => vm.Active == true)
                .OrderBy(vm => vm.PricePerDay)
                .Take(size)
                .Select(VehicleDtoSelector)
                .ToListAsync();

            return vehicles;
        }

        public async Task<PagedResult<VehicleDto>> GetVehicles(VehicleModelFilter filter = null)
        {
            if (filter == null)
            {
                filter = new VehicleModelFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<VehicleModel> vehicleModels = _dbContext.VehicleModels;

            if (!string.IsNullOrWhiteSpace(filter?.VehicleType))
                vehicleModels = vehicleModels.Where(vm => vm.VehicleType.Contains(filter.VehicleType));
            if (filter?.MinPricePerDay != null)
                vehicleModels = vehicleModels.Where(vm => vm.PricePerDay >= filter.MinPricePerDay);
            if (filter?.MaxPricePerDay != null)
                vehicleModels = vehicleModels.Where(vm => vm.PricePerDay <= filter.MaxPricePerDay);

            if (filter.Active)
            {
                vehicleModels = vehicleModels.Where(vm => vm.Active == filter.Active);
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = vehicleModels.Count();
                vehicleModels = vehicleModels.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await vehicleModels
                .Select(VehicleDtoSelector)
                .ToListAsync();

            return new PagedResult<VehicleDto>
            {
                Total = Total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Results = results
            };
        }

        public async Task<VehicleModelDto> GetVehicle(int? id)
        {
            return await _dbContext.VehicleModels
                .Where(vm => vm.Id == id)
                .Select(VehicleModelDtoSelector)
                .SingleOrDefaultAsync();
        }

        public async Task<VehicleModelDetailsDto> GetVehicleModel(int? id)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Include(v => v.Cars)
                .Include(r => r.Ratings)
                .Where(vm => vm.Id == id)
                .Select(VehicleModelDetailsDtoSelector)
                .SingleOrDefaultAsync();

            return vehicleModel;
        }

        public async Task CreateVehicleModel(VehicleModelInputDto vehicleModelDto)
        {
            var vehicle = new VehicleModel
            {
                VehicleType = vehicleModelDto.VehicleType,
                PricePerDay = vehicleModelDto.PricePerDay ?? 0,
                NumberOfDoors = vehicleModelDto.NumberOfDoors ?? 0,
                NumberOfSeats = vehicleModelDto.NumberOfSeats ?? 0,
                Active = vehicleModelDto.Active,
                AirConditioning = vehicleModelDto.AirConditioning,
                Automatic = vehicleModelDto.Automatic,
                VehicleUrl = string.Empty
            };

            _dbContext.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            if (vehicleModelDto.Picture != null || vehicleModelDto.Picture.Length > 0)
            {
                var file = vehicleModelDto.Picture;
                var upload = Path.Combine(_hosting.WebRootPath, "images");
                var extension = Path.GetExtension(file.FileName);
                var fileName = Path.GetFileName(file.FileName);
                if (file.Length > 0)
                {

                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string myfileName = name + '_' + vehicle.Id + extension;
                    using (var filestream = new FileStream(Path.Combine(upload, myfileName), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                        vehicle.VehicleUrl = myfileName;
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task EditVehicle(VehicleModelDto vehicleModelDto, IFormFile Picture)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Where(vm => vm.Id == vehicleModelDto.Id)
                .SingleOrDefaultAsync();

            vehicleModel.VehicleType = vehicleModelDto.VehicleType;
            vehicleModel.PricePerDay = vehicleModelDto.PricePerDay;
            vehicleModel.NumberOfDoors = vehicleModelDto.NumberOfDoors;
            vehicleModel.NumberOfSeats = vehicleModelDto.NumberOfSeats;
            vehicleModel.Active = vehicleModelDto.Active;
            vehicleModel.AirConditioning = vehicleModelDto.AirConditioning;
            vehicleModel.Automatic = vehicleModelDto.Automatic;

            if (Picture != null && Picture.Length > 0)
            {
                var file = Picture;
                var upload = Path.Combine(_hosting.WebRootPath, "images");
                var extension = Path.GetExtension(file.FileName);
                string fileName = Path.GetFileName(file.FileName);
                string currentName = vehicleModel.VehicleUrl;

                if (currentName != null && currentName.Length > 0)
                {
                    string libary = Path.Combine(_hosting.WebRootPath, "images");
                    string fullPath = Path.Combine(libary, currentName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                if (file.Length > 0)
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string myfileName = name + '_' + vehicleModel.Id + extension;

                    using (var fileStream = new FileStream(Path.Combine(upload, myfileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        vehicleModel.VehicleUrl = myfileName;
                    }
                }
            }

            _dbContext.Attach(vehicleModel).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task EditVehicleModel(VehicleModelEditDto vehicleModelDto)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Where(vm => vm.Id == vehicleModelDto.Id)
                .SingleOrDefaultAsync();

            vehicleModel.VehicleType = vehicleModelDto.VehicleType;
            vehicleModel.PricePerDay = vehicleModelDto.PricePerDay.Value;
            vehicleModel.NumberOfDoors = vehicleModelDto.NumberOfDoors.Value;
            vehicleModel.NumberOfSeats = vehicleModelDto.NumberOfSeats.Value;
            vehicleModel.Active = vehicleModelDto.Active;
            vehicleModel.AirConditioning = vehicleModelDto.AirConditioning;
            vehicleModel.Automatic = vehicleModelDto.Automatic;

            if (vehicleModelDto.Picture != null && vehicleModelDto.Picture.Length > 0)
            {
                var file = vehicleModelDto.Picture;
                var upload = Path.Combine(_hosting.WebRootPath, "images");
                var extension = Path.GetExtension(file.FileName);
                string fileName = Path.GetFileName(file.FileName);
                string currentName = vehicleModel.VehicleUrl;

                if (currentName != null && currentName.Length > 0)
                {
                    string libary = Path.Combine(_hosting.WebRootPath, "images");
                    string fullPath = Path.Combine(libary, currentName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                if (file.Length > 0)
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string myfileName = name + '_' + vehicleModel.Id + extension;

                    using (var fileStream = new FileStream(Path.Combine(upload, myfileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        vehicleModel.VehicleUrl = myfileName;
                    }
                }
            }

            _dbContext.Attach(vehicleModel).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVehicle(int? id)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Where(vm => vm.Id == id)
                .Include(vm => vm.Cars)
                .Include(vm => vm.Reservations)
                .SingleOrDefaultAsync();

            var cars = await _dbContext.Cars
                .Where(c => c.VehicleModelId == id)
                .Include(c => c.VehicleModel)
                .ToListAsync();

            var filename = vehicleModel.VehicleUrl;

            if (filename != null && filename.Length > 0)
            {
                string libary = Path.Combine(_hosting.WebRootPath, "images");
                string fullPath = Path.Combine(libary, filename);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _dbContext.RemoveRange(cars);
            _dbContext.Remove(vehicleModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> VehicleModelHasReservations(int? id)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Where(vm => vm.Id == id)
                .Include(vm => vm.Reservations)
                .SingleOrDefaultAsync();

            if (vehicleModel.Reservations.Any())
            {
                return true;
            }

            return false;
        }

        public bool VehicleModelExists(int? id)
        {
            return _dbContext.VehicleModels.Any(e => e.Id == id);
        }

        public async Task<VehicleModelDeleteDto> GetVehicleModelDelete(int? id)
        {
            var vehicleModel = await _dbContext.VehicleModels
                .Include(vm => vm.Reservations)
                .Where(vm => vm.Id == id)
                .Select(vm => new VehicleModelDeleteDto
                {
                    Id = vm.Id,
                    VehicleType = vm.VehicleType,
                    PricePerDay = vm.PricePerDay,
                    VehicleUrl = vm.VehicleUrl,
                    NumberOfDoors = vm.NumberOfDoors,
                    NumberOfSeats = vm.NumberOfSeats,
                    Automatic = vm.Automatic,
                    AirConditioning = vm.AirConditioning,
                    Active = vm.Active,
                    HasReservation = vm.Reservations.Any()
                })
                .SingleOrDefaultAsync();

            return vehicleModel;
        }
    }
}
