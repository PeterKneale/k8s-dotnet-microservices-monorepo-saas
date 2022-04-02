namespace Registration.Messages
{
    public class RegistrationRequest
    {
        public string AccountId { get; init; }
        public string StoreId { get; init; }
        public string UserId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Reference { get; init; }
    }
}