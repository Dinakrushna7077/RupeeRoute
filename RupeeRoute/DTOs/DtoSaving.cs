namespace RupeeRoute.API.DTOs
{
    public class DtoSaving
    {
        public int SavingId { get; set; }
        public int UserId { get; set; }
        public long Amount{ get; set; }
        public string? Note { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
    }

}
