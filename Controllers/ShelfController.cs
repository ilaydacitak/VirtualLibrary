using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DenemeSWE.Models;

namespace DenemeSWE.Controllers
{
    public class ShelfController : Controller
    {
        IheBookEntities5 db = new IheBookEntities5();
        // GET: Shelf
        public ActionResult Index()
        {
            return View();
        }
        //login işlemi yapan kullanıcıya ait kitapları getir
        

        

        [HttpGet]
        public async Task<ActionResult> Myshelf(string search)
        {

            if (Session["id"] != null)
            {
                var id = Convert.ToInt32(Session["id"]);
                ViewData["Shelf"] = search;
                var query = db.Shelf.Where(x => x.user_id == id);
                if (!String.IsNullOrEmpty(search))
                {
                    if (search.Length >= 3)
                    {
                        query = query.Where(x => x.title.Contains(search));
                    }
                    else
                    {
                        ViewData["LoginFlag"] = "You must enter at least 3 characters to search!!";
                        return View(await query.AsNoTracking().ToListAsync());
                    }
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        //kütüphanedeki var olan kitabı kaldırma
        [HttpGet]
        public ActionResult RemoveBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RemoveBook(string bookId)
        {
            // shelfteki seçili kitabı kaldır
            var book = db.Shelf.Where(x => x.book_id == bookId).FirstOrDefault();
            db.Shelf.Remove(book);
            db.SaveChanges();




            return RedirectToAction("Myshelf");

           
        }

    }/**/
}