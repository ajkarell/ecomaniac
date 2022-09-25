namespace Ecomaniac.Api.Validation;

public static class TransactionValidator
{
    public static bool Validate(Transaction transaction, out string? errorMessage)
    {
        errorMessage = null;

        if (transaction.Amount == 0)
        {
            errorMessage = "Transaction amount cannot be zero.";
            return false;
        }

        if (transaction.Date > DateTimeOffset.Now)
        {
            errorMessage = "Transaction date cannot be in the future.";
            return false;
        }

        return true;
    }
}