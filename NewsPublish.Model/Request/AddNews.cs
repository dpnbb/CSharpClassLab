﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class AddNews
    {
        public int NewsClassifyID { get; set; }
        public string Image { get; set; }
        public string Contents { get; set; }
        public string Remark { get; set; }
        public string Title { get; set; }
    }
}
