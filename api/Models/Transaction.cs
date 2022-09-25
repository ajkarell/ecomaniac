using System.ComponentModel.DataAnnotations;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTimeOffset Date { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public string Description { get; set; }
}