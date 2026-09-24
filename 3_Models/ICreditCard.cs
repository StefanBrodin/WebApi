namespace Models;

public interface ICreditCard
{
    public CardIssues Issuer { get; set; }
    public Guid CreditCardId { get; set; }
    public string Number { get; set; }
    public string ExpirationYear { get; set; }
    public string ExpirationMonth { get; set; }
    public string CardHolderName { get; set; }
    public bool Seeded { get; set; }
}


