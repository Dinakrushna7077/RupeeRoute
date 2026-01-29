namespace RupeeRoute.API.DTOs
{
    public class DtoCreateSaving
    {
        public int SavingId { get; set; }
        //public int UserId { get; set; }
        public long Amount { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
