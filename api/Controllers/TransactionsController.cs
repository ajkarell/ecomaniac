using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecomaniac.Api.Controllers;

[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public TransactionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
    {
        var transactions = await _db.Transactions
            .OrderBy(x => x.Date)
            .ToListAsync();

        if (!transactions.Any())
            return Ok(Enumerable.Empty<Transaction>());

        return Ok(transactions);
    }

    [HttpGet("transactions/{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction([Required] int id)
    {
        var transaction = await _db.Transactions
            .FindAsync(id);

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpPost("transactions")]
    public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
    {
        _db.Add(transaction);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }
}