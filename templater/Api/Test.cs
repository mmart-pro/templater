using Microsoft.AspNetCore.Mvc;
using templater.Classes;

namespace templater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Test : ControllerBase
    {
        private readonly ILogger<Test> _logger;
        private readonly Filler _filler;

        public Test(
            ILogger<Test> logger,
            Filler filler
            )
        {
            _logger = logger;
            _filler = filler;
        }

        // GET: Test
        [HttpGet]
        public IActionResult Get()
        {
            _filler.Fill(new contracts.TemplaterRequest());
            return Ok("test"); //  View();
        }

        //// GET: Test/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Test/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Test/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Test/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Test/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Test/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Test/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
