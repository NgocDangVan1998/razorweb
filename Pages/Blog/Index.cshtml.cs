using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageIdentity.Models;

namespace RazorPageIdentity.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageIdentity.Models.MyBlogContext _context;

        public IndexModel(RazorPageIdentity.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }

        public const int ITEM_PER_PAGE = 10;
        [BindProperty(SupportsGet =true,Name ="p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public async Task OnGetAsync(string SearchString)
        {
            //Article = await _context.articles.ToListAsync();

            int totalArticle = await _context.articles.CountAsync();

            countPages = (int)Math.Ceiling((double)totalArticle / ITEM_PER_PAGE);
            if(currentPage < 1)
            {
                currentPage = 1;
            }
            if(currentPage > countPages)
            {
                currentPage = countPages;
            }

            var qr = from a in _context.articles
                      orderby a.Created descending
                      select a;
            if (!string.IsNullOrEmpty(SearchString))
            {
                Article =  qr.Where(q => q.Title.Contains(SearchString)).ToList();
            }
            else
            {
                Article = await qr.ToListAsync();
            }
            
        }
    }
}
