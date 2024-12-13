using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Music_Shop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LatestAlbum> Albums { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Загрузка данных из представления
            Albums = await _context.LatestAlbums.ToListAsync();
        }
    }
}


