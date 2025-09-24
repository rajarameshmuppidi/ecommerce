using Microsoft.Identity.Client;

namespace EcommercePlatform.Services
{

    public interface ITransientService
    {
        Guid GetGuid();
    }

    public class TransientService:ITransientService
    {
        public Guid Id { get; set; }

        public TransientService()
        {
            Id = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return Id;
        }
    }
}
