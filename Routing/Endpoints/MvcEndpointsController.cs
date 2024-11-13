using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Templates
{
    public class MvcEndpointsController : Controller
    {
        // GET: MvcTemplateController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MvcTemplateController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MvcTemplateController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MvcTemplateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MvcTemplateController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MvcTemplateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MvcTemplateController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MvcTemplateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
