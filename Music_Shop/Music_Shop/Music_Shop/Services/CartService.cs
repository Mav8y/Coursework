using Microsoft.EntityFrameworkCore;

public class CartService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private string CartId => _httpContextAccessor.HttpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();

    public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        // Убедимся, что сессия имеет идентификатор корзины
        _httpContextAccessor.HttpContext.Session.SetString("CartId", CartId);
    }

    public async Task AddToCart(int albumId, int quantity = 1)
    {
        var cartId = CartId; // Уникальный идентификатор сессии
        var cartItem = await _context.CartItems
            .Include(ci => ci.Album)
            .FirstOrDefaultAsync(ci => ci.AlbumId == albumId && ci.CartId == cartId);

        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                CartId = cartId,
                AlbumId = albumId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _context.SaveChangesAsync();
    }


    public async Task<List<CartItem>> GetCartItems()
    {
        return await _context.CartItems
            .Where(ci => ci.CartId == CartId)
            .Include(ci => ci.Album)
            .ToListAsync();
    }

    public async Task RemoveFromCart(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCart()
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.CartId == CartId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }
}
