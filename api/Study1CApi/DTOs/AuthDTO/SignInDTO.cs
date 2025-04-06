namespace Study1CApi.DTOs.AuthDTO
{
    public class SignInDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserSurname { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? UserPatronymic { get; set; }

        public bool IsFirst { get; set; }

        public string Token { get; set; } = String.Empty;
    }
}
