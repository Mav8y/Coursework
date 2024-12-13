using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CartId { get; set; } // Уникальный идентификатор корзины

    [Required]
    public int AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album Album { get; set; }

    [Required]
    public int Quantity { get; set; } 
}
