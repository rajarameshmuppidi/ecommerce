namespace EcommercePlatform.CustomExceptions
{
    public class OutOfStockException(string message, Exception? inner =  null) : Exception(message, inner)
    {
    }
}
