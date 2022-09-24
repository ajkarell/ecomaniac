using System.ComponentModel.DataAnnotations;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal ChangeInBalance { get; set; }
    public string Description { get; set; }
}