namespace EcommercePlatform.CustomExceptions
{
    public class ProductModifiedException(string message, Exception? inner = null) : Exception(message, inner)
    {
    }
}
