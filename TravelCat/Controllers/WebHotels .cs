﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class WebHotelsController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: web_activities
        public ActionResult Index()
        {
            TaiwanViewModel model = new TaiwanViewModel()
            {
                collections_detail = db.collections_detail.ToList(),
                comment = db.comment.ToList(),
                hotel_north = db.hotel.Where(m => m.city.Contains("臺北市") || m.city.Contains("新北市") || m.city.Contains("基隆市") || m.city.Contains("新竹市") || m.city.Contains("桃園市") || m.city.Contains("新竹縣") || m.city.Contains("宜蘭縣")).OrderByDescending(m => db.comment.Where(s => s.tourism_id == m.hotel_id).Count()).Take(6).ToList(),
                hotel_middle = db.hotel.Where(m => m.city.Contains("臺中市") || m.city.Contains("苗栗縣") || m.city.Contains("彰化縣") || m.city.Contains("南投縣") || m.city.Contains("雲林縣")).OrderByDescending(m => db.comment.Where(s => s.tourism_id == m.hotel_id).Count()).Take(6).ToList(),
                hotel_south = db.hotel.Where(m => m.city.Contains("高雄市") || m.city.Contains("臺南市") || m.city.Contains("嘉義市") || m.city.Contains("屏東縣") || m.city.Contains("澎湖縣")).OrderByDescending(m => db.comment.Where(s => s.tourism_id == m.hotel_id).Count()).Take(6).ToList(),
                hotel_East = db.hotel.Where(m => m.city.Contains("花蓮縣") || m.city.Contains("臺東縣")).OrderByDescending(m => db.comment.Where(s => s.tourism_id == m.hotel_id).Count()).Take(6).ToList(),
                hotel_island = db.hotel.Where(m => m.city.Contains("金門縣") || m.city.Contains("連江縣")).OrderByDescending(m => db.comment.Where(s => s.tourism_id == m.hotel_id).Count()).Take(6).ToList(),

            };
            return View(model);
        }

        // GET: web_activities/Details/5
        public ActionResult Details(string id, int page = 1)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            destinationsViewModel model = new destinationsViewModel()
            {
                hotel = db.hotel.Where(m => m.hotel_id == id).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == id).OrderByDescending(m=>m.comment_date).ToList(),
                message = db.message.Where(m => m.tourism_id == id).OrderByDescending(m => m.msg_time).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
                collections_detail = db.collections_detail.Where(m => m.tourism_id == id).ToList(),
            };
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.tourismId = id;
            List<spot> spot = db.spot.Where(m => m.district == model.hotel.district).OrderByDescending(m => m.spot_id).Take(3).ToList();
            ViewBag.tourismId = id;
            ViewBag.spot = spot;
            return View(model);
        }     
        
    }
}