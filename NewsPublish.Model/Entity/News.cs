using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class News
    {
        public News()
        {
            NewsComment = new HashSet<NewsComment>();
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public int NewsClassifyID { get; set; }
        public string Image { get; set; }
        public string Contents { get; set; }
        public DateTime PublishDate { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 外键的类，将新闻类别的实体加进来
        /// </summary>
        public virtual NewsClassify NewsClassify { get; set; }
        /// <summary>
        /// 添加新闻评论，一个新闻对应多个评论   
        /// </summary>
        public virtual ICollection<NewsComment> NewsComment { get; set; }

    }
}
