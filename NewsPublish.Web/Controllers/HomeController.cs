using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsPublish.Model.Response;
using NewsPublish.Service;
using NewsPublish.Web.Models;

namespace NewsPublish.Web.Controllers
{
    public class HomeController : Controller
    {
        private NewsService _newsService;
        private BannerService _bannerService;
        public HomeController(NewsService newsService, BannerService bannerService)
        {
            this._newsService = newsService;
            this._bannerService = bannerService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "首页";
            return View(_newsService.GetNewsClassifyList());
        }
        [HttpGet]
        public JsonResult GetBanner()
        {
            return Json(_bannerService.GetBannerList());
        }

        [HttpGet]
        public JsonResult GetTotalNews()
        {
            return Json(_newsService.GetNewsCount(c => true));
        }

        [HttpGet]
        public JsonResult GetHomeNews()
        {
            return Json(_newsService.GetNewsList(c => true, 6));
        }
        [HttpGet]
        public JsonResult GetNewsCommentNews()
        {
            return Json(_newsService.GetNewsCommentList(c => true, 5));
        }
        [HttpGet]
        public JsonResult SrarchOneNews(string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
            {
                return Json(new ResponseModel { Code = 0, Result = "关键字不能为空！" });
            }
            else
            {
                return Json(_newsService.GetSearchNews(c => c.Title.Contains(keyWord)));
            }
        }
        public ActionResult Wrong()
        {
            ViewData["Title"] = "404";
            return View(_newsService.GetNewsClassifyList());
        }
    }
}
