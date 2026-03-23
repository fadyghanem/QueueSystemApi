using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServiceController(AppDbContext context)
    {
        _context = context;
    }

    // 🔐 Only logged-in users
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateService(CreateServiceDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // 🔥 Check if user OWNS the business
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.Id == dto.BusinessId);

        if (business == null)
            return NotFound("Business not found");

        if (business.OwnerId != userId)
            return Forbid("You do not own this business");

        var service = new Service
        {
            Name = dto.Name,
            DurationMinutes = dto.DurationMinutes,
            Price = dto.Price,
            BusinessId = dto.BusinessId
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        var result = new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            DurationMinutes = service.DurationMinutes,
            Price = service.Price,
            BusinessId = service.BusinessId
        };

        return Ok(result);
    }

    // 📋 Get all services for a business
    [HttpGet("{businessId}")]
    public async Task<IActionResult> GetServices(int businessId)
    {
        var services = await _context.Services
            .Where(s => s.BusinessId == businessId)
            .ToListAsync();

        var result = services.Select(s => new ServiceDto
        {
            Id = s.Id,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            Price = s.Price,
            BusinessId = s.BusinessId
        }).ToList();

        return Ok(result);
    }
}