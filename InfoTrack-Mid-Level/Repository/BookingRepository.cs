using InfoTrack_Mid_Level.Data;
using Microsoft.EntityFrameworkCore;

namespace InfoTrack_Mid_Level.Repository
{
    public class BookingRepository : IBookingRepository
    {
        public readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Check if booking is between 09:00 and 16:00
        /// </summary>
        /// <param name="bookingTime"></param>
        /// <returns></returns>
        public bool CheckIfBookingTimeIsBetweenHours(DateTime bookingTime)
        {
            //Initialise Start and end times
            DateTime today = DateTime.Today;
            DateTime startTime = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0);
            DateTime endTime = new DateTime(today.Year, today.Month, today.Day, 16, 0, 0);

            //Check if request time is outside of booking hours
            if (bookingTime.TimeOfDay < startTime.TimeOfDay || bookingTime.TimeOfDay > endTime.TimeOfDay) return false;

            return true;
        }
        /// <summary>
        /// Check if booking time overlaps with other bookings - check within 59 minute range of all existing bookins
        /// </summary>
        /// <param name="bookingTime"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfBookingTimeOverlapsWithOtherBookingAsync(DateTime bookingTime)
        {
            BookingResponse rs = new BookingResponse();
            List<Bookings> bookings = await GetAllBookings();
            var existingBookingsWithOverlappingTimes = bookings.Where(b => (bookingTime >= b.BookingTime && bookingTime <= b.BookingTime.AddMinutes(59))).ToList();

            if (existingBookingsWithOverlappingTimes.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add new booking to database
        /// Returns a unique ID in the form of GUID
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public async Task<string> AddBookings(BookingRequest rq)
        {
            string bookingId = Guid.NewGuid().ToString();
            _context.Bookings.Add(new Bookings
            {
                BookingTime = rq.bookingTime,
                BookingGuid = bookingId
            });
            await _context.SaveChangesAsync();

            return bookingId;
        }

        /// <summary>
        /// Returns all bookins
        /// </summary>
        /// <returns></returns>
        private async Task<List<Bookings>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }
    }
}
