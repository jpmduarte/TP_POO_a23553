namespace Program
{
    public class ParkingRecord
    {
        public Vehicle Vehicle { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public decimal ParkingFee { get; set; }

        public ParkingRecord(Vehicle vehicle, DateTime entryTime, DateTime exitTime, decimal parkingFee)
        {
            Vehicle = vehicle;
            EntryTime = entryTime;
            ExitTime = exitTime;
            ParkingFee = parkingFee;
        }
    }
}