using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Shop.Services;

public class MusicModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;

    public MusicModel(ApplicationDbContext context, CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    // Свойство для хранения списка альбомов
    public List<Album> Albums { get; set; }

    // Обработчик для GET-запроса
    public async Task OnGetAsync()
    {
        // Получение всех альбомов из базы данных
        Albums = await _context.Albums.ToListAsync();
    }

    // Обработчик для добавления в корзину
    public async Task<IActionResult> OnPostAddToCartAsync(int albumId)
    {
        await _cartService.AddToCart(albumId);
        return RedirectToPage();
    }
}



