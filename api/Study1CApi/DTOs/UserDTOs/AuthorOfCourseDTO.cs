namespace Study1CApi.DTOs.UserDTOs
{
    public class AuthorOfCourseDTO
    {
        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }
    }
}
