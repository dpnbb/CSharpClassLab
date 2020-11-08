using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Response {
    public class CommentModel {
        public int ID { get; set; }
        public string NewsName { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
        public string Floor { get; set; }
    }
}
