namespace ClassLibrary1
{
    public interface IAzureTokenValidator
    {
        bool ValidateToken(
            string token);
    }
}
