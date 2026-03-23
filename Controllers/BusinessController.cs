using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly AppDbContext _context;

    public BusinessController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Only logged-in users
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBusiness(CreateBusinessDto dto)
    {
        // Get user ID from JWT
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return Unauthorized();

        // Upgrade user to Owner if not already
        if (user.Role != "Owner")
            user.Role = "Owner";

        var business = new Business
        {
            Name = dto.Name,
            OwnerId = userId
        };

        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();

        var result = new BusinessDto
        {
            Id = business.Id,
            Name = business.Name,
            OwnerId = business.OwnerId
        };

        return Ok(result);
    }

    // ✅ Get all businesses for current user
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyBusinesses()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var businesses = await _context.Businesses
            .Where(b => b.OwnerId == userId)
            .ToListAsync();

        var result = businesses.Select(b => new BusinessDto
        {
            Id = b.Id,
            Name = b.Name,
            OwnerId = b.OwnerId
        }).ToList();

        return Ok(result);
    }
}