using FitBlaze.Data;
using FitBlaze.Features.Wiki.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitBlaze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly PageService _pageService;

        public PagesController(PageService pageService)
        {
            _pageService = pageService;
        }

        // GET: api/Pages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> GetPages()
        {
            return Ok(await _pageService.GetPagesAsync());
        }

        // GET: api/Pages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Page>> GetPage(int id)
        {
            var page = await _pageService.GetPageByIdAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        // POST: api/Pages
        [HttpPost]
        public async Task<ActionResult<Page>> PostPage(Page page)
        {
            var createdPage = await _pageService.AddPageAsync(page);
            return CreatedAtAction(nameof(GetPage), new { id = createdPage.Id }, createdPage);
        }

        // PUT: api/Pages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage(int id, Page page)
        {
            if (id != page.Id)
            {
                return BadRequest();
            }

            var updatedPage = await _pageService.UpdatePageAsync(page);

            if (updatedPage == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Pages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            var result = await _pageService.DeletePageAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}