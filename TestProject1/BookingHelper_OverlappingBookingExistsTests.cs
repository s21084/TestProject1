using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja;
using TestNinja.Mocking;

namespace TestProject1
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingExistsTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;
        [SetUp]
        public void SetUp() 
        { 
        _existingBooking = new Booking
            {
                Id = 2,
                ArrivalDate = ArriveOn(2022,1,15),
                DepartureDate = DepartOn(2022, 1, 20),
                Reference = "a"
            };
            
            _repository = new Mock<IBookingRepository>();
            _repository.Setup(r => r.GetActiveBookings(1))
                      .Returns(new List<Booking> {
                      _existingBooking
                      }.AsQueryable());
        }

        [Test]
        public void BookingStartAndFinishedBeforeExistingBooking_ReturnEmptyString()
        {
            

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate,10),
                DepartureDate = Before(_existingBooking.ArrivalDate,1)
            }, _repository.Object);

            Assert.That(result, Is.Empty);
        }
        [Test]
        public void BookingStartsBeforeAndFinishedInTheMiddleOfAnExistingBooking_ReturnsExistingBookingsReference()
        {


            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, 10),
                DepartureDate = After(_existingBooking.ArrivalDate, 1)
            }, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }



        private DateTime Before(DateTime dateTime, int days = 1) {
            return dateTime.AddDays(-days);
        }
        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }
        private DateTime ArriveOn(int year, int month, int day) {
            return new DateTime(year, month, day, 0, 0, 0);
        }
        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

    }
}