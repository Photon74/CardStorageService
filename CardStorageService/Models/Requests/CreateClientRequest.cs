namespace CardStorageService.Models.Requests
{
    public class CreateClientRequest
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? Patronymic { get; set; }
    }
}
