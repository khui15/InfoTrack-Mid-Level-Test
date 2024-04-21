using InfoTrack_Mid_Level.Data;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack_Mid_Level.Repository
{
    public interface IBookingRepository
    {
        bool CheckIfBookingTimeIsBetweenHours(DateTime bookingTime);
        Task<bool> CheckIfBookingTimeOverlapsWithOtherBookingAsync(DateTime bookingTime);
        Task<string> AddBookings(BookingRequest rq);
    }
}
