namespace ConcertBooking.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }

        
        public string UserId { get; set; }
        public AppUser User { get; set; }

        
        public int ConcertId { get; set; }
        public Concert Concert { get; set; }

        
        public string Status { get; set; }
    }
}