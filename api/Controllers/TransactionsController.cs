using System.ComponentModel.DataAnnotations;
using Ecomaniac.Api.Validation;
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
            .OrderByDescending(x => x.Date)
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
    public async Task<ActionResult<Transaction>> PostTransaction([Required] Transaction transaction)
    {
        if (!TransactionValidator.Validate(transaction, out var errorMessage))
        {
            return BadRequest(errorMessage);
        }

        _db.Add(transaction);

        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }

    [HttpPut("transactions")]
    public async Task<ActionResult<Transaction>> PutTransaction([Required] Transaction transaction)
    {
        if (transaction.Id < 1)
        {
            return BadRequest("Id must be positive.");
        }

        var existing = await _db.Transactions.FindAsync(transaction.Id);
        if (existing != null)
        {
            if (!TransactionValidator.Validate(transaction, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            existing.Date = transaction.Date;
            existing.Amount = transaction.Amount;
            existing.Description = transaction.Description;

            await _db.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("transactions/{id}")]
    public async Task<ActionResult> DeleteTransaction([Required] int id)
    {
        var transaction = await _db.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _db.Remove(transaction);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}