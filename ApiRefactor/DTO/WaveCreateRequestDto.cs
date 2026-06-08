namespace ApiRefactor.DTO
{
    public class WaveCreateRequestDto
    {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public DateTime WaveDate { get; set; }
    }
}
