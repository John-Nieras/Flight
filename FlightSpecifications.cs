using Xunit;
using Domain;
using FluentAssertions;

namespace FlightTest
{
    public class FlightSpecifications
    {
        //We use this
        [Theory]
        [InlineData(3,1,2)]//First scenario in theory
        public void Booking_reduces_the_number_of_seats(int seatCapacity, int numberOfSeats, int remainingNumberOfSeats)
        {
            var flight = new Flight(seatCapacity: seatCapacity);

            flight.Book("johnfranklin@tutorials.com",numberOfSeats);

            flight.RemainingNumberOfSeats.Should().Be(remainingNumberOfSeats);
        }
        //Original tester
        [Fact]
        public void Booking_reduces_the_number_of_seats1()
        {
            var flight = new Flight(seatCapacity: 3);

            flight.Book("johnfranklin@tutorials.com", 1);

            flight.RemainingNumberOfSeats.Should().Be(2);
        }

        //Version 2 
        //It's okay to repeat the code in testing..
        [Fact]
        public void Booking_reduces_the_number_of_seats2()
        {
            var flight = new Flight(seatCapacity: 3);

            flight.Book("johnfranklin@tutorials.com", 1);

            flight.RemainingNumberOfSeats.Should().Be(2);
        }

        [Fact]

        public void Avoids_overbooking()
        {
            //Given
            var flight = new Flight(seatCapacity: 3);
            //When
            var error = flight.Book("johnfranklin@tutorial.com", 4);
            //Then
            error.Should().BeOfType<OverbookingError>();
        }

        [Fact]

        public void Book_flights_sucessfully()
        {
            var flight = new Flight(seatCapacity: 3);
            var error = flight.Book("johnfranklin@tutorial.com", 1);
            error.Should().BeNull();
        }

        [Fact]

        public void Remembers_bookings()
        {
            var flight = new Flight(seatCapacity: 150);
            flight.Book(passengerEmail: "a@b.com",numberOfSeats: 4);
            flight.BookingList.Should().ContainEquivalentOf(new Booking("a@b.com", 4));
        }

        [Theory]
        [InlineData(3,1,1,3)]

        public void Canceling_bookings_frees_up_the_seats(
           int initialCapacity,
           int numberOfSeatsToBook,
           int numberOfSeatsToCancel,
           int remainingNumberOfSeats
            )
        {
            //given
            var flight = new Flight(initialCapacity);
            flight.Book(passengerEmail: "a@b.com", numberOfSeats: numberOfSeatsToBook);
            //when
            flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: numberOfSeatsToCancel);
            //then
            flight.RemainingNumberOfSeats.Should().Be(remainingNumberOfSeats);
        }

        [Fact]

        public void Doesnt_cancel_bookings_for_passengers_who_have_not_booked()
        {
            var flight = new Flight(3);
            var error = flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: 2);
            error.Should().BeOfType<BookingNotFoundError>();
        }

        [Fact]

        public void Returns_null_when_successfully_cancels_a_booking()
        {
            var flight = new Flight(3);
            flight.Book(passengerEmail: "a@b.com", numberOfSeats: 1);
            var error = flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: 1);
            error.Should().BeNull();
        }

    }
}