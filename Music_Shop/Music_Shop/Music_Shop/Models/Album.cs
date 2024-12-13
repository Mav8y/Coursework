using System.ComponentModel.DataAnnotations;

public class Album
{
    public int Id { get; set; }

    [Required]
    public string Artist { get; set; }

    [Required]
    public string AlbumTitle { get; set; }

    [Required]
    public string Genre { get; set; }

    [Required]
    public decimal Price { get; set; }

    public byte[] AlbumCover { get; set; } // Обложка альбома

    public List<Track> Tracks { get; set; } = new();
}

public class Track
{
    public int Id { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public byte[] FileData { get; set; } // MP3-файл

    public string Hash { get; set; } // Хэш для исключения повторов

    public int AlbumId { get; set; }
    public Album Album { get; set; }
}

