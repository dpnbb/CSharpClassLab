using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class NewsClassify
    {
        public NewsClassify()
        {
            News = new HashSet<News>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 添加新闻列表，一个类别对应多个新闻
        /// </summary>
        public virtual ICollection<News> News { get; set; }
    }
}
