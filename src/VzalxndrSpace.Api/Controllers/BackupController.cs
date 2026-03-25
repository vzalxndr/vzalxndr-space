using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VzalxndrSpace.Api.Services;

namespace VzalxndrSpace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BackupController : ControllerBase
{
    private readonly IBackupService _backupService;

    public BackupController(IBackupService backupService)
    {
        _backupService = backupService;
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportDatabase()
    {
        var bytes = await _backupService.ExportDatabaseAsync();
        
        return File(bytes, "application/json", $"vzalxndr_backup_{DateTime.UtcNow:yyyyMMdd_HHmm}.json");
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportDatabase(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "File is empty" });
        }

        using var stream = file.OpenReadStream();
        var importedCount = await _backupService.ImportDatabaseAsync(stream);

        return Ok(new { message = "Database successfully restored!", goalsCount = importedCount });
    }
}