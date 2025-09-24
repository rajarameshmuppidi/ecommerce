namespace EcommercePlatform.Services
{
    public interface ISingleTon
    {
        Guid GetGuid();
    }
    public class SingleTon:ISingleTon
    {
        public Guid Id { get; set; }

        public SingleTon() {
            Id = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return Id;
        }
    }
}
