using CarRental.Dal.Dtos;
using CarRental.Dal.Entities;
using CarRental.Dal.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.Services
{
    public interface IReservationService
    {

        Task CreateReservation(Reservation reservation);

        Task EditReservation(int? id, int? carid);

        Task CancelReservation(int? id);

        Task DeleteReservation(int? id);

        Task<PagedResult<ReservationIndexHeader>> GetReservations(ReservationFilter filter);

        Task<PagedResult<ReservationListHeader>> GetReservations(ReservationListFilter filter, int? userid);

        IEnumerable<Reservation> GetReservations(int? userid);

        Task DeletedUserReservations(int? userid);

        Task<ReservationHeader> GetReservation(int? id);

        bool ReservationExists(int? id);
    }
}
