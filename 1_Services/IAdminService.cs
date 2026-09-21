using Models;

namespace Services;

public interface IAdminService
{
    public List<IQuote> Quotes();
    public List<string> EncryptedQuotes();
    public IQuote DecryptedQuote(string encryptedQuote);
}
