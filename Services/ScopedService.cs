namespace EcommercePlatform.Services
{
    public interface IScopedService
    {
        Guid GetGuid();
    }

    public class ScopedService:IScopedService
    {
        public Guid Id { get; set; }

        public ScopedService()
        {
            Id = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return Id;
        }
    }
}
