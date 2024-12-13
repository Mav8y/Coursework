using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AlbumDetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AlbumDetailsModel> _logger;

    public AlbumDetailsModel(ApplicationDbContext context, ILogger<AlbumDetailsModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public AlbumDetailsViewModel AlbumDetails { get; set; }
    public List<AlbumDetailsViewModel> Tracks { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Получение данных альбома
            AlbumDetails = await _context.AlbumDetailsView
                .Where(a => a.AlbumId == id)
                .FirstOrDefaultAsync();

            if (AlbumDetails == null)
            {
                _logger.LogWarning($"Album with ID {id} not found.");
                return NotFound("Album not found.");
            }

            // Получение всех треков для альбома
            Tracks = await _context.AlbumDetailsView
                .Where(a => a.AlbumId == id && a.TrackId != 0 && a.TrackData != null)
                .Select(a => new AlbumDetailsViewModel
                {
                    TrackId = a.TrackId,
                    TrackName = a.TrackName,
                    TrackData = a.TrackData
                })
                .ToListAsync();

            if (Tracks == null || Tracks.Count == 0)
            {
                _logger.LogInformation($"No tracks found for album ID {id}.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading album details for ID {id}: {ex.Message}");
            return StatusCode(500, "An error occurred while loading album details.");
        }

        return Page();
    }
}




