using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CartModel : PageModel
{
    private readonly CartService _cartService;

    public CartModel(CartService cartService)
    {
        _cartService = cartService;
    }

    public List<CartItem> CartItems { get; set; }

    public async Task OnGetAsync()
    {
        // Загрузка содержимого корзины
        CartItems = await _cartService.GetCartItems();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
    {
        await _cartService.RemoveFromCart(cartItemId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClearAsync()
    {
        await _cartService.ClearCart();
        return RedirectToPage();
    }
}



