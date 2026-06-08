namespace ApiRefactor.DTO
{
    public class WaveResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime WaveDate { get; set; }
    }
}
