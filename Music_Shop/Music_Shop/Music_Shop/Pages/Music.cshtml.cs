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

    // �������� ��� �������� ������ ��������
    public List<Album> Albums { get; set; }

    // ���������� ��� GET-�������
    public async Task OnGetAsync()
    {
        // ��������� ���� �������� �� ���� ������
        Albums = await _context.Albums.ToListAsync();
    }

    // ���������� ��� ���������� � �������
    public async Task<IActionResult> OnPostAddToCartAsync(int albumId)
    {
        await _cartService.AddToCart(albumId);
        return RedirectToPage();
    }
}



