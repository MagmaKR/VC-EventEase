namespace VCEventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime Booking_Start_Date { get; set; }
        public DateTime Booking_End_Date { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

    }
}
