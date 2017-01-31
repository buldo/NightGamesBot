using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buldo.Ngb.Bot.EnginesManagement;
using Buldo.Ngb.Web.Data;
using Buldo.Ngb.Web.Models.EnginesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Buldo.Ngb.Web.Controllers
{
    [Authorize]
    public class EnginesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnginesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new EnginesListViewModel();
            await _context.Engines.ForEachAsync(o => viewModel.Engines.Add(new EngineViewModel(o)));
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var toRem = _context.Engines.Find(id);
            if (toRem != null)
            {
                _context.Engines.Remove(toRem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [FromForm]string name,
            [FromForm]string address,
            [FromForm]string login,
            [FromForm]string password)
        {
            var info = new EngineInfo {Address = address, Login = login, Name = name, Password = password};
            _context.Engines.Add(info);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
