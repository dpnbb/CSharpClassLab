using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NewsPublish.Service
{
    public class NewsService
    {
        private Db_Help _db;
        public NewsService(Db_Help db)
        {
            this._db = db;
        }
        /// <summary>
        /// 添加一个新闻类别
        /// </summary>
        /// <param name="newsClassify"></param>
        /// <returns></returns>
        public ResponseModel AddNewsClassify(AddNewsClassify newsClassify)
        {
            var exit = _db.NewsClassify.FirstOrDefault(c => c.Name == newsClassify.Name) != null;
            if (exit)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该类别已存在！"
                };
            }
            var classify = new NewsClassify
            {
                Name = newsClassify.Name,
                Sort = newsClassify.Sort,
                Remark = newsClassify.Remark
            };
            _db.NewsClassify.Add(classify);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "类别添加成功！"
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "类别添加失败！"
                };
            }
        }
        /// <summary>
        /// 获取一个新闻类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetOneNewsClassify(int id)
        {
            var classify = _db.NewsClassify.Find(id);
            if (classify == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该类别不存在！"
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "新闻类别获取成功！",
                    Data = new NewsClassifyModel
                    {
                        ID = classify.ID,
                        Name = classify.Name,
                        Sort = classify.Sort,
                        Remark = classify.Remark,
                    }
                };
            }
        }
        /// <summary>
        /// 根据条件获取一个NewsClassify实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private NewsClassify GetOneNewsClassify(Expression<Func<NewsClassify, bool>> where)
        {
            return _db.NewsClassify.FirstOrDefault(where);
        }
        /// <summary>
        /// 编辑一个新闻类别
        /// </summary>
        /// <param name="newsClassify"></param>
        /// <returns></returns>
        public ResponseModel EditNewsClassify(EditNewsClassify newsClassify)
        {
            var classify = this.GetOneNewsClassify(c => c.ID == newsClassify.ID);
            if (classify == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该类别不存在！",
                };
            }
            classify.Name = newsClassify.Name;
            classify.Sort = newsClassify.Sort;
            classify.Remark = newsClassify.Remark;
            _db.NewsClassify.Update(classify);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "类别修改成功！",
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "类别修改失败！",
                };
            }
        }
        /// <summary>
        /// 获取类别集合
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetNewsClassifyList()
        {
            var classifies = _db.NewsClassify.ToList().OrderByDescending(c => c.Sort).ToList();
            var response = new ResponseModel
            {
                Code = 200,
                Result = "获取类别集合成功！",
                Data = new List<NewsClassifyModel>(),
            };
            foreach (var classify in classifies)
            {
                response.Data.Add(new NewsClassifyModel
                {
                    ID = classify.ID,
                    Name = classify.Name,
                    Sort = classify.Sort,
                    Remark = classify.Remark,
                });
            }
            return response;
        }
        /// <summary>
        /// 添加一个新闻
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public ResponseModel AddNews(AddNews news)
        {
            var exit = _db.News.FirstOrDefault(c => c.Title == news.Title) != null;
            if (exit)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该新闻已存在！"
                };
            }
            var insert_news = new News
            {
                Title = news.Title,
                NewsClassifyID = news.NewsClassifyID,
                Remark = news.Remark,
                Image = news.Image,
                Contents = news.Contents,
                PublishDate = DateTime.Now,
            };
            _db.News.Add(insert_news);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "新闻添加成功！",
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "新闻添加失败！",
                };
            }
        }
        /// <summary>
        /// 获取一个新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetOneNews(int id)
        {
            var news = _db.News.Include("NewsClassify").Include("NewsComment").FirstOrDefault(c => c.ID == id);
            if (news == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该新闻不存在！",
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "新闻获取成功！",
                    Data = new NewsModel
                    {
                        ID = news.ID,
                        Title = news.Title,
                        Remark = news.Remark,
                        Contents = news.Contents,
                        Image = news.Image,
                        PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                        ClassifyName = news.NewsClassify.Name,
                        CommentCount = news.NewsComment.Count(),
                    }
                };
            }
        }
        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DeleteOneNews(int id)
        {
            var news = _db.News.FirstOrDefault(c => c.ID == id);
            if (news == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "该新闻不存在！",
                };
            }
            _db.News.Remove(news);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "一条新闻删除成功！",
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "一条新闻删除失败！",
                };
            }
        }
        /// <summary>
        /// 分页查询新闻
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="wheres"></param>
        /// <returns></returns>
        public ResponseModel NewsPageQuery(int pagesize, int pageIndex, out int total, List<Expression<Func<News, bool>>> wheres)
        {
            var list = _db.News.Include("NewsClassify").Include("NewsComment");
            foreach (var item in wheres)
            {
                list = list.Where(item);
            }
            total = list.Count();

            var pageData = list.OrderByDescending(c => c.PublishDate).Skip(pagesize * (pageIndex - 1)).Take(pagesize).ToList();
            var response = new ResponseModel
            {
                Code = 200,
                Result = "分页新闻获取成功！",
            };
            response.Data = new List<NewsModel>();
            foreach (var model in pageData)
            {
                response.Data.Add(new NewsModel
                {
                    ID = model.ID,
                    ClassifyName = model.NewsClassify.Name,
                    Title = model.Title,
                    Image = model.Image,
                    CommentCount = model.NewsComment.Count(),
                    Contents = model.Contents.Length > 50 ? model.Contents.Substring(0, 50) + "..." : model.Contents.Trim(),
                    PublishDate = model.PublishDate.ToString("yyyy-MM-dd"),
                    Remark = model.Remark,
                });
            }
            return response;
        }
        /// <summary>
        /// 查询新闻列表，首页需要使用
        /// </summary>
        /// <param name="where"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResponseModel GetNewsList(Expression<Func<News, bool>> where, int topCount)
        {
            var list = _db.News.Include("NewsClassify").Include("NewsComment").Where(where).OrderByDescending(c => c.PublishDate).Take(topCount);
            var response = new ResponseModel
            {
                Code = 200,
                Result = "新闻获取成功！",
            };
            response.Data = new List<NewsModel>();
            foreach (var model in list)
            {
                response.Data.Add(new NewsModel
                {
                    ID = model.ID,
                    ClassifyName = model.NewsClassify.Name,
                    Title = model.Title,
                    Image = model.Image,
                    CommentCount = model.NewsComment.Count(),
                    Contents = model.Contents.Length > 50 ? model.Contents.Substring(0, 50) : model.Contents,
                    PublishDate = model.PublishDate.ToString("yyyy-MM-dd"),
                    Remark = model.Remark,
                });
            }
            return response;
        }
        /// <summary>
        /// 获取最新新闻评论的新闻集合
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResponseModel GetNewsCommentList(Expression<Func<News, bool>> where, int topCount)
        {
            var list = _db.News.Include("NewsClassify").Include("NewsComment").Where(where).OrderByDescending(c => c.PublishDate);
            var response = new ResponseModel
            {
                Code = 200,
                Result = "最新评论新闻获取成功！",
            };
            response.Data = new List<NewsModel>();
            foreach (var model in list)
            {
                response.Data.Add(new NewsModel
                {
                    ID = model.ID,
                    ClassifyName = model.NewsClassify.Name,
                    Title = model.Title,
                    Image = model.Image,
                    CommentCount = model.NewsComment.Count(),
                    Contents = model.Contents.Length > 50 ? model.Contents.Substring(0, 50) : model.Contents,
                    PublishDate = model.PublishDate.ToString("yyyy-MM-dd"),
                    Remark = model.Remark,
                });
            }
            return response;
        }
        /// <summary>
        /// 搜索一条新闻
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel GetSearchNews(Expression<Func<News, bool>> where)
        {
            var news = _db.News.Where(where).FirstOrDefault();
            if (news == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "新闻搜索失败！",
                };
            }
            else
            {
                return new ResponseModel
                {
                    Code = 200,
                    Result = "新闻搜索成功！",
                    Data = news.ID,
                };
            }
        }
        /// <summary>
        /// 获取新闻数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel GetNewsCount(Expression<Func<News, bool>> where)
        {
            int count = _db.News.Where(where).Count();
            return new ResponseModel
            {
                Code = 200,
                Result = "获取新闻数量成功！",
                Data = count,
            };
        }
        /// <summary>
        /// 获取推荐新闻
        /// </summary>
        /// <param name="newsid"></param>
        /// <returns></returns>
        public ResponseModel GetRecommendNewsList(int newsid)
        {
            var news = _db.News.FirstOrDefault(c => c.ID == newsid);
            if (news == null)
            {
                return new ResponseModel
                {
                    Code = 0,
                    Result = "新闻不存在！",
                };
            }
            var newsList = _db.News.Include("NewsComment").Where(c => c.NewsClassifyID == news.NewsClassifyID && c.ID != newsid).OrderByDescending(c => c.PublishDate).OrderByDescending(c => c.NewsComment.Count()).Take(6).ToList();
            var response = new ResponseModel
            {
                Code = 200,
                Result = "推荐新闻获取成功！",
            };
            response.Data = new List<NewsModel>();
            foreach (var model in newsList)
            {
                response.Data.Add(new NewsModel
                {
                    ID = model.ID,
                    ClassifyName = model.NewsClassify.Name,
                    Title = model.Title,
                    Image = model.Image,
                    CommentCount = model.Contents.Count(),
                    Contents = model.Contents.Length > 50 ? model.Contents.Substring(0, 50) : model.Contents,
                    PublishDate = model.PublishDate.ToString("yyyy-MM-dd"),
                    Remark = model.Remark,
                });
            }
            return response;
        }
    }
}
