namespace ApiRefactor.Domain.Entities
{
    public class Wave
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime WaveDate { get; set; }


    }
}