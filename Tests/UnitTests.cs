using InfoTrack_Mid_Level;
using InfoTrack_Mid_Level.Controllers;
using InfoTrack_Mid_Level.Data;
using InfoTrack_Mid_Level.Repository;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace Tests
{
    public class UnitTests
    {
        public BookingRepository getBookingRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new AppDbContext(optionsBuilder.Options);
            return new BookingRepository(context);
        }

        public BookingRepository _bookingRepository;
        public UnitTests()
        {
            _bookingRepository = getBookingRepository();
        }
        #region
        // Check basic before and after start and end time to ensure correct logic is working
        [Fact]
        public void CheckForBookingTimeBeforeStartTime()
        {
            Assert.False(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(8)));
            Assert.False(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(5)));
            Assert.False(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(8).AddMinutes(59)));
        }
        [Fact]
        public void CheckForBookingTimeAfterStartTime()
        {
            Assert.False(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(16).AddMinutes(1)));
            Assert.False(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(7)));
        }
        [Fact]
        public void CheckForBookingTimeBeforeEndTime()
        {
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(16)));
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(14)));
        }
        [Fact]
        public void CheckForBookingTimeAfterEndTime()
        {
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(9)));
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(11)));
        }
        #endregion

        [Fact]
        public async Task CheckIfBookingWillConflict()
        {
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(10)));
            BookingRequest rq = new BookingRequest()
            {
                bookingTime = DateTime.Today.AddHours(10),
                name = "Test Booking"
            };
            var bookingId = _bookingRepository.AddBookings(rq);
            //ensure bookingId is not null
            Assert.NotEqual(bookingId, null);

            Assert.True(await _bookingRepository.CheckIfBookingTimeOverlapsWithOtherBookingAsync(DateTime.Today.AddHours(10)));
        }
        [Fact]
        public async Task CheckIfBookingWillNotConflictAndBook()
        {
            Assert.True(_bookingRepository.CheckIfBookingTimeIsBetweenHours(DateTime.Today.AddHours(10)));
            BookingRequest rq = new BookingRequest()
            {
                bookingTime = DateTime.Today.AddHours(10),
                name = "Test Booking"
            };
            var bookingId = _bookingRepository.AddBookings(rq);
            //ensure bookingId is not null
            Assert.NotEqual(bookingId, null);

            Assert.False(await _bookingRepository.CheckIfBookingTimeOverlapsWithOtherBookingAsync(DateTime.Today.AddHours(11)));
        }
    }
}