﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Controllers
{
    public class NewsController : Controller
    {
        private NewsService _newsService;
        private CommentService _commentService;
        public NewsController(NewsService newsService, CommentService commentService)
        {
            this._newsService = newsService;
            this._commentService = commentService;
        }
        public IActionResult Classify(int id)
        {
            if (id <= 0)
            {
                Response.Redirect("/Home/Index");
            }
            var classify = _newsService.GetOneNewsClassify(id);
            ViewData["ClassifyName"] = "首页";
            ViewData["NewsList"] = new ResponseModel();
            ViewData["NewCommentNews"] = new ResponseModel();
            ViewData["Title"] = "首页";
            if (classify.Code == 0)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewData["ClassifyName"] = classify.Data.Name;
                var newsList = _newsService.GetNewsList(c => c.NewsClassifyID == id, 6);
                ViewData["NewsList"] = newsList;
                var newCommentNews = _newsService.GetNewsCommentList(c => c.NewsClassifyID == id, 5);
                ViewData["NewCommentNews"] = newCommentNews;
                ViewData["Title"] = classify.Data.Name;
            }
            return View(_newsService.GetNewsClassifyList());
        }
        public IActionResult Detail(int id)
        {
            ViewData["Title"] = "详情页";
            if (id <= 0)
            {
                Response.Redirect("/Home/Index");
            }
            var news = _newsService.GetOneNews(id);
            ViewData["News"] = new ResponseModel();
            ViewData["RecommandNews"] = new ResponseModel();
            ViewData["Comments"] = new ResponseModel();
            if (news.Code == 0)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewData["Title"] = news.Data.Title + "-详情页";
                ViewData["News"] = news;
                var recommendNews = _newsService.GetRecommendNewsList(id);
                ViewData["RecommandNews"] = recommendNews;
                var comments = _commentService.GetCommentList(c => c.NewsID == id);
                ViewData["Comments"] = comments;
            }
            return View(_newsService.GetNewsClassifyList());
        }
        [HttpPost]
        public JsonResult AddComment(AddComment comment)
        {
            if (comment.NewsID <= 0)
            {
                return Json(new ResponseModel { Code = 0, Result = "新闻不存在！" });
            }
            if (string.IsNullOrEmpty(comment.Contents))
            {
                return Json(new ResponseModel { Code = 0, Result = "内容不能为空！" });
            }
            return Json(_commentService.AddComment(comment));
        }
    }
}
