namespace CardStorageService.Models.Requests
{
    public class CreateCardRequest
    {
        public int ClientId { get; set; }

        public string CardNo { get; set; } = "0000-0000-0000-0000-0000";

        public string? Name { get; set; }

        public string? CVV2 { get; set; }

        public DateTime ExpDate { get; set; }
    }
}
