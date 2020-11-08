using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class NewsComment
    {
        public int ID { get; set; }
        public int NewsID { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
        public virtual News News { get; set; }
    }
}
