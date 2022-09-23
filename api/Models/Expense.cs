using System.ComponentModel.DataAnnotations;

public class Expense
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal ChangeInBalance { get; set; }
}