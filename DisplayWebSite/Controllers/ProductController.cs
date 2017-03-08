using DisplayWebSite.Models.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DisplayWebSite.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        // TODO:IFベースに置き換えてFactoryからDBContextを取得するように変更
        // TODO:サニタイズ
        // TODO:ValidateAntiForgeryTokenの使い方を調べる
        public ActionResult Index(string artistName)
        {
            if (artistName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var context = new ProductContext(); 
            var data = context.GetAllByArtistName(artistName);

            ViewBag.artistName = artistName;
            return View(data);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
