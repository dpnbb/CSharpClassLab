using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Request {
    public class AddComment {
        public int NewsID { get; set; }
        public string Contents { get; set; }
    }
}
