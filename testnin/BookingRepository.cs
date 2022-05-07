using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja
{
    public interface IBookingRepository {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }
    class BookingRepository : IBookingRepository
    {

        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null) 
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Status != "Cancelled");
            //b.Id != booking.Id && 

            if (excludedBookingId.HasValue)
                bookings = bookings.Where(b => b.Id != excludedBookingId.Value);

            return bookings;
        }

    }
}
