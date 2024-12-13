using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NAudio.Wave;
using System.Security.Cryptography;

[Authorize(Roles = "Admin")]
public class UploadMusicModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public UploadMusicModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Artist { get; set; }

    [BindProperty]
    public string AlbumTitle { get; set; }

    [BindProperty]
    public string Genre { get; set; }

    [BindProperty]
    public decimal Price { get; set; }

    [BindProperty]
    public IFormFile AlbumCover { get; set; }

    [BindProperty]
    public List<IFormFile> Tracks { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Сохранение обложки альбома
        byte[] coverData;
        using (var ms = new MemoryStream())
        {
            await AlbumCover.CopyToAsync(ms);
            coverData = ms.ToArray();
        }

        // Создание альбома
        var album = new Album
        {
            Artist = Artist,
            AlbumTitle = AlbumTitle,
            Genre = Genre,
            Price = Price,
            AlbumCover = coverData
        };
        _context.Albums.Add(album);

        foreach (var track in Tracks)
        {
            // Обрезка MP3 до 0.99 MB
            byte[] trimmedFile = await TrimMp3File(track);

            // Генерация хэша для исключения повторов
            string hash = ComputeFileHash(trimmedFile);

            if (await _context.Tracks.AnyAsync(t => t.Hash == hash))
            {
                ModelState.AddModelError("", $"Track {track.FileName} already exists.");
                continue;
            }

            var newTrack = new Track
            {
                FileName = track.FileName,
                FileData = trimmedFile,
                Hash = hash,
                Album = album
            };

            _context.Tracks.Add(newTrack);
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("/UploadMusic");
    }

    private async Task<byte[]> TrimMp3File(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var reader = new Mp3FileReader(memoryStream);
        using var writer = new MemoryStream();
        var trimmedBytes = new LimitMp3Stream(reader, 0.99 * 1024 * 1024);
        await trimmedBytes.CopyToAsync(writer);

        return writer.ToArray();
    }

    private string ComputeFileHash(byte[] fileData)
    {
        using var sha256 = SHA256.Create();
        return BitConverter.ToString(sha256.ComputeHash(fileData)).Replace("-", "");
    }
}

