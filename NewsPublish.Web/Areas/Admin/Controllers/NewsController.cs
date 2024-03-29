﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private NewsService _newsService;
        private IHostingEnvironment _host;
        public NewsController(NewsService newsService, IHostingEnvironment host)
        {
            this._newsService = newsService;
            this._host = host;
        }
        public ActionResult Index()
        {
            var newClassifies = _newsService.GetNewsClassifyList();
            return View(newClassifies);
        }

        [HttpGet]
        public JsonResult GetNews(int pageIndex, int pageSize, int classifyID, string keyWord)
        {
            List<Expression<Func<News, bool>>> wheres = new List<Expression<Func<News, bool>>>();
            if (classifyID > 0)
            {
                wheres.Add(c => c.NewsClassifyID == classifyID);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                wheres.Add(c => c.Title.Contains(keyWord));
            }
            int total = 0;
            var news = _newsService.NewsPageQuery(pageSize, pageIndex, out total, wheres);
            return Json(new { total = total, data = news.Data });
        }

        public ActionResult NewsAdd()
        {
            var newClassifies = _newsService.GetNewsClassifyList();
            return View(newClassifies);
        }

        [HttpPost]
        public async Task<JsonResult> AddNews(AddNews news, IFormCollection collection)
        {
            if (news.NewsClassifyID <= 0 || string.IsNullOrEmpty(news.Title) || (string.IsNullOrEmpty(news.Contents)))
            {
                return Json(new ResponseModel { Code = 0, Result = "参数有误！" });
            }
            else
            {
                var files = collection.Files;
                if (files.Count > 0)
                {
                    string webRootPath = _host.WebRootPath;
                    string relativeDirPath = "\\NewsPic";
                    string absolutePath = webRootPath + relativeDirPath;
                    string[] fileTypes = new string[] { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };//定义允许上传的图片格式
                    string extension = Path.GetExtension(files[0].FileName);
                    if (fileTypes.Contains(extension))
                    {
                        if (!Directory.Exists(absolutePath))
                        {
                            Directory.CreateDirectory(absolutePath);
                        }
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        var filePath = absolutePath + "\\" + fileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await files[0].CopyToAsync(stream);
                        }
                        news.Image = "/NewsPic/" + fileName;
                        return Json(_newsService.AddNews(news));

                    }
                    else
                    {
                        return Json(new ResponseModel { Code = 0, Result = "图片格式有误！" });
                    }
                }
                else
                {
                    return Json(new ResponseModel { Code = 0, Result = "请上传图片文件！" });
                }
            }
        }

        [HttpPost]
        public JsonResult DelNews(int id)
        {
            if (id < 0)
            {
                return Json(new ResponseModel { Code = 0, Result = "新闻不存在！" });
            }
            else
            {
                return Json(_newsService.DeleteOneNews(id));
            }
        }
        #region--新闻类别--
        public ActionResult NewsClassify()
        {
            var newsClassifies = _newsService.GetNewsClassifyList();
            return View(newsClassifies);
        }
        public ActionResult NewsClassifyAdd()
        {
            return View();
        }
        [HttpPost]
        public JsonResult NewsClassifyAdd(AddNewsClassify newsClassify)
        {
            if (string.IsNullOrEmpty(newsClassify.Name))
            {
                return Json(new ResponseModel { Code = 0, Result = "请输入新闻类别名称！" });
            }
            else
            {
                return Json(_newsService.AddNewsClassify(newsClassify));
            }
        }
        public ActionResult NewsClassifyEdit(int id)
        {
            return View(_newsService.GetOneNewsClassify(id));
        }
        [HttpPost]
        public JsonResult NewsClassifyEdit(EditNewsClassify newsClassify)
        {
            if (string.IsNullOrEmpty(newsClassify.Name))
            {
                return Json(new ResponseModel { Code = 0, Result = "请输入新闻类别名称！" });
            }
            else
            {
                return Json(_newsService.EditNewsClassify(newsClassify));
            }
        }
        #endregion

    }
}
