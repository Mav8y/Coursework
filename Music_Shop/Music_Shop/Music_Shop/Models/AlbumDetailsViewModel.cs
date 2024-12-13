public class AlbumDetailsViewModel
{
    public int? AlbumId { get; set; }
    public string Artist { get; set; }
    public string AlbumTitle { get; set; }
    public string Genre { get; set; }
    public decimal? Price { get; set; }
    public byte[] AlbumCover { get; set; }

    public int? TrackId { get; set; }
    public string TrackName { get; set; }
    public byte[] TrackData { get; set; }
}



