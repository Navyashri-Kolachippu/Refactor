namespace ApiRefactor.DTO
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } 
        public int ExpiresIn { get; set; }
    }
}
