namespace EcommercePlatform.CustomExceptions
{
    public class PriceChangedException(string message, Exception? inner = null) : Exception(message, inner)
    {
    }
}
