namespace RupeeRoute.API.DTOs
{
    public class DtoCreateSaving
    {
        public int SavingId { get; set; }
        public int UserId { get; set; }
        public byte[] EncryptedAmount { get; set; }
        public byte[] EncryptedNote { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
