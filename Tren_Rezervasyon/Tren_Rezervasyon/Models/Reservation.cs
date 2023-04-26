namespace Tren_Rezervasyon.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string TrainName { get; set; }
        public string VagonName { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
