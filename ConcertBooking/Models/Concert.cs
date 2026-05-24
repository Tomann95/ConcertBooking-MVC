namespace ConcertBooking.Models
{
    public class Concert
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableSeats { get; set; }

        
        public List<Reservation> Reservations { get; set; }
    }
}