using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Db_Help db = new Db_Help();
            NewsService newsService = new NewsService(db);
            ResponseModel responseModel = newsService.AddNewsClassify(new AddNewsClassify { Name = "军事", Remark = "军事", Sort = 1 });
            System.Console.WriteLine(responseModel.Result);
        }
        //[TestMethod]
        //public void TestMethod2()
        //{
        //    Db_Help db = new Db_Help();
        //    NewsService newsService = new NewsService(db);
        //    CommentService commentService = new CommentService(db, newsService);
        //    ResponseModel responseModel = commentService.GetCommentList(c => true);
        //    System.Console.WriteLine(responseModel.Result);
        //}
    }
}