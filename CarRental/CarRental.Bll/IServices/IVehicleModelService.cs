﻿using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IVehicleModelService
    {
        IEnumerable<VehicleModel> GetVehicles();

        IEnumerable<VehicleModel> GetActiveVehicles();

        Task<IEnumerable<VehicleDto>> GetBestOffers(int size);

        Task<PagedResult<VehicleDto>> GetVehicles(VehicleModelFilter filter = null);

        Task<VehicleModelDto> GetVehicle(int? id);

        Task CreateVehicle(VehicleModelDto vehicleModelDto, IFormFile Picture);

        Task EditVehicle(VehicleModelDto vehicleModelDto, IFormFile Picture);

        Task DeleteVehicle(int? id);

        Task<bool> VehicleModelHasReservations(int? id);

        bool VehicleModelExists(int? id);
    }
}
