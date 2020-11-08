using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            this._commentService = commentService;
        }
        // GET: CommentController
        public ActionResult Index()
        {
            return View(_commentService.GetCommentList(c=>true));
        }
        [HttpPost]
        public JsonResult DelComment(int id)
        {
            if (id < 0)
            {
                return Json(new ResponseModel { Code = 0, Result = "参数有误！" });
            }
            else
            {
                return Json(_commentService.DeleteComment(id));
            }
        }
    }
}
