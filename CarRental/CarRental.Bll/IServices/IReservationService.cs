using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
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

        Task<IEnumerable<ReservationListHeader>> GetReservationListHeaders(int? userid);
    }
}
