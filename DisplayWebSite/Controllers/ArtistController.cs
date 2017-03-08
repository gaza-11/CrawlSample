using DisplayWebSite.Data;
using DisplayWebSite.Models.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisplayWebSite.Controllers
{
    public class ArtistController : Controller
    {
        // GET: Artist
        /// <summary>
        /// 選択済みアーティスト一覧の表示
        /// </summary>
        /// <returns>一覧画面の表示</returns>
        public ActionResult Index()
        {
            var context = new ArtistContext();
            var data = context.GetAll();

            return View(data);
        }

        // アーティストの登録画面への遷移
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        /// <summary>
        /// お気に入りアーティストの登録
        /// </summary>
        /// <param name="artist">アーティスト情報</param>
        /// <returns>一覧ページへ遷移</returns>
        [HttpPost]
        public ActionResult Create(Artist artist)
        {
            try
            {
                var context = new ArtistContext();
                context.Add(artist, "TestUser");

                //TODO Productデータがなかったら取得スレッドを動かすかどうか検討
                //TODO その場合Redis登録時にすでに存在するかの判定が必要

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Delete/5
        // 選択済みアーティスト一覧からの削除
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Artist/Delete/5
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
