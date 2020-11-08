using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NewsPublish.Service
{
    /// <summary>
    /// Banner服务
    /// </summary>
    public class BannerService
    {
        private Db_Help _db;
        public BannerService(Db_Help db)
        {
            this._db = db;
        }
        /// <summary>
        /// 添加Banner
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public ResponseModel AddBanner(AddBanner banner)
        {
            var ba = new Banner { AddTime = DateTime.Now, Image = banner.Image, Url = banner.Url, Remark = banner.Remark };
            _db.Banner.Add(ba);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "Banner添加成功！" };
            }
            else
            {
                return new ResponseModel { Code = 0, Result = "Banner添加失败" };
            }
        }
        /// <summary>
        /// 获取Banner集合
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetBannerList()
        {
            var banners = _db.Banner.ToList().OrderByDescending(c => c.ID);
            var response = new ResponseModel();
            response.Code = 200;
            response.Result = "获取Banner集合成功！";
            response.Data = new List<BannerModel>();
            foreach (var banner in banners)
            {
                response.Data.Add(new BannerModel
                {
                    ID = banner.ID,
                    Image = banner.Image,
                    Url = banner.Url,
                    Remark = banner.Remark
                });
            }
            return response;
        }
        public ResponseModel DeleteBanner(int banner_id)
        {
            var banner = _db.Banner.Find(banner_id);
            if (banner == null)
            {
                return new ResponseModel { Code = 0, Result = "Banner不存在！" };
            }
            _db.Banner.Remove(banner);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "删除Banner成功！" };
            }
            return new ResponseModel { Code = 0, Result = "Banner删除失败！" };
        }
    }
}
