using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == dto.ServiceId);

        if (service == null)
            return NotFound("Service not found");

        var endTime = dto.StartTime.AddMinutes(service.DurationMinutes);

        // 🔥 Prevent overlapping appointments
        var conflict = await _context.Appointments.AnyAsync(a =>
            a.ServiceId == dto.ServiceId &&
            a.StartTime < endTime &&
            dto.StartTime < a.StartTime.AddMinutes(service.DurationMinutes)
        );

        if (conflict)
            return BadRequest("Time slot already taken");

        var appointment = new Appointment
        {
            ServiceId = dto.ServiceId,
            UserId = userId,
            BusinessId = service.BusinessId,
            StartTime = dto.StartTime,
            Status = "Scheduled"
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var result = new AppointmentDto
        {
            Id = appointment.Id,
            ServiceId = appointment.ServiceId,
            StartTime = appointment.StartTime,
            Status = appointment.Status
        };

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMyAppointments()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var appointments = await _context.Appointments
            .Where(a => a.UserId == userId)
            .ToListAsync();

        var result = appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            ServiceId = a.ServiceId,
            StartTime = a.StartTime,
            Status = a.Status
        }).ToList();

        return Ok(result);
    }
}