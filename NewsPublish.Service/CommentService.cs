using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;

namespace NewsPublish.Service
{
    public class CommentService
    {
        private Db_Help _db;
        private NewsService _newsService;
        public CommentService(Db_Help db, NewsService newsService)
        {
            this._db = db;
            this._newsService = newsService;
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public ResponseModel AddComment(AddComment comment)
        {
            var news = _newsService.GetOneNews(comment.NewsID);
            if (news.Code == 0)
            {
                return new ResponseModel { Code = 0, Result = "新闻不存在！" };
            }
            else
            {
                var com = new NewsComment { AddTime = DateTime.Now, NewsID = comment.NewsID, Contents = comment.Contents };
                _db.NewsComment.Add(com);
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    return new ResponseModel
                    {
                        Code = 200,
                        Result = "新闻评论添加成功！",
                        Data = new
                        {
                            contents = comment.Contents,
                            floor = "#" + (int)((int)news.Data.CommentCount + 1),
                            addTime = DateTime.Now.ToString("yyyy/MM/d HH:mm:ss"),
                        }
                    };
                }
                else
                {
                    return new ResponseModel { Code = 0, Result = "新闻评论添加失败！" };
                }
            }
        }
        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DeleteComment(int id)
        {
            var comment = _db.NewsComment.Find(id);
            if (comment == null)
            {
                return new ResponseModel { Code = 0, Result = "评论不存在！" };
            }
            else
            {
                _db.NewsComment.Remove(comment);
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    return new ResponseModel
                    {
                        Code = 200,
                        Result = "新闻评论删除成功！",
                    };
                }
                else
                {
                    return new ResponseModel { Code = 0, Result = "新闻评论删除失败！" };
                }
            }
        }
        /// <summary>
        /// 获取评论集合
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ResponseModel GetCommentList(Expression<Func<NewsComment, bool>> where)
        {
            var news = _db.News.ToList();//加了这行莫名奇妙地读取到了news实体，不加这个读不到
            var comments = _db.NewsComment.Include("News").Where(where).OrderBy(c => c.AddTime).ToList();
            var response = new ResponseModel();
            response.Code = 200;
            response.Result = "评论获取成功！";
            response.Data = new List<CommentModel>();
            int floor = 1;
            foreach (var comment in comments)
            {
                response.Data.Add(new CommentModel
                {
                    ID = comment.ID,
                    NewsName = comment.News.Title,
                    Contents = comment.Contents,
                    AddTime = comment.AddTime,
                    Remark = comment.Remark,
                    Floor = "#" + floor,
                });
                floor++;
            }
            response.Data.Reverse();
            return response;
        }
    }
}
