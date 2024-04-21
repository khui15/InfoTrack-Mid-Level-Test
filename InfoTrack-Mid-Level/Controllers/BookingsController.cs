using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfoTrack_Mid_Level.Data;
using Microsoft.AspNetCore.RateLimiting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using InfoTrack_Mid_Level.Repository;

namespace InfoTrack_Mid_Level.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBookingRepository _bookingsRepository;

        public BookingsController(AppDbContext context, IBookingRepository bookingsRepository)
        {
            _context = context;
            _bookingsRepository = bookingsRepository;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<List<Bookings>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        // DUMMY API
        [HttpGet("{id}")]
        public async Task<ActionResult<Bookings>> GetBookings(int id)
        {
            return NoContent();
        }

        // PUT: api/Bookings/5
        // DUMMY API
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookings(int id, Bookings bookings)
        {
            return NoContent();
        }

        [HttpPost]
        [EnableRateLimiting("rateLimit")]
        public async Task<ActionResult<BookingResponse>> PostBookings(BookingRequest rq)
        {
            if (!_bookingsRepository.CheckIfBookingTimeIsBetweenHours(rq.bookingTime)) return BadRequest();
            if (await _bookingsRepository.CheckIfBookingTimeOverlapsWithOtherBookingAsync(rq.bookingTime)) return Conflict();
            BookingResponse rs = new BookingResponse();
            rs.bookingId = await _bookingsRepository.AddBookings(rq);
            return Ok(rs);
        }

        // DELETE: api/Bookings/5
        // Dummy API
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookings(int id)
        {
            return NoContent();
        }

        private bool BookingsExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
