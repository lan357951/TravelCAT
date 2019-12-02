﻿using X.PagedList;
using X.PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class SearchController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        int pageSize = 15;

        //GET: Search
        public ActionResult Index(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            SearchViewModel model = new SearchViewModel();

            if (!String.IsNullOrEmpty(q))
            {
                model = new SearchViewModel
                {
                    activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList()
                };
            }
            else
            {
                model = new SearchViewModel
                {
                    activity = db.activity.OrderBy(s => s.activity_id).ToList(),
                    hotel = db.hotel.OrderBy(s => s.hotel_id).ToList(),
                    restaurant = db.restaurant.OrderBy(s => s.restaurant_id).ToList(),
                    spot = db.spot.OrderBy(s => s.spot_id).ToList()
                };
            }
            return View(model);
            //return View(model.activity.OrderBy(m => m.activity_id).ToPagedList(page, pageSize));
        }
        [ChildActionOnly]
        public ActionResult _Activity(string q = null, int page = 1, string Sortby = "htol", string city = null, string comment_rating = null, string[] travel_partner = null, string[] travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            SearchViewModel model = new SearchViewModel {
                activity = db.activity.OrderBy(s => s.activity_title).ToList()
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                if (!String.IsNullOrEmpty(city))
                {
                    //model.activity = model.activity.Where(s => s.city.Contains(city) || s.district.Contains(city)|| s.activity_intro.Contains(city) || s.activity_title.Contains(city)).ToList();
                    for (int i = 0; i < model.activity.Count; i++)
                    {
                        if (String.IsNullOrEmpty(model.activity[i].city)&& String.IsNullOrEmpty(model.activity[i].district)) 
                        {
                            model.activity.RemoveAt(i);
                        }else if (!model.activity[i].city.Contains(city) && !model.activity[i].district.Contains(city)) 
                        {
                            model.activity.RemoveAt(i);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(comment_rating))
                {
                    string id;
                    int rating = int.Parse(comment_rating); //查詢條件轉型
                    for (int i = 0; i < model.activity.Count; i++)
                    {
                        id = model.activity[i].activity_id;
                        int cmt_count = db.comment.Where(s => s.tourism_id == id).Count();
                        if (cmt_count == 0)
                        {
                            model.activity.RemoveAt(i);
                        }
                        else
                        {
                            //找每個活動的平均評分
                            double rating_avg = db.comment.Where(s => s.tourism_id == id).Average(r => r.comment_rating);
                            if (rating_avg < rating)
                            {
                                model.activity.RemoveAt(i);     //移除評分過小者
                                i = i - 1;
                            }
                        }
                    }
                }

                //if (!String.IsNullOrEmpty(comment_stay_total))
                //{
                //    for (int i = 0; i < model.activity.Count; i++)
                //    {
                //        //找每個活動的平均時間
                //        double stay_avg = db.comment.Where(s => s.tourism_id == model.activity[i].activity_id).ToList().Average(r => r.comment_stay_total);
                //        if
                //    }
                //}
                if (travel_partner != null)
                {
                    for (int i = 0; i < model.activity.Count; i++)
                    {
                        bool activity_exist = false;
                        model.comment = db.comment.Where(s => s.tourism_id == model.activity[i].activity_id).ToList();
                        for (int j = 0; j < model.comment.Count; j++)   //找對於單個活動的所有評論內是否有含搜尋選項
                        {
                            var result = model.comment.Where(s => s.travel_partner.Contains(travel_partner[j])).FirstOrDefault();
                            if (result != null)
                            {
                                activity_exist = true;  //只要有一項就代表活動符合資格
                            }
                        }
                        if (!activity_exist)
                        {
                            model.activity.RemoveAt(i);     //不符合資格的移除
                            i = i - 1;
                        }
                    }
                }
                if (travel_month != null)
                {
                    for (int i = 0; i < model.activity.Count; i++)
                    {
                        bool activity_exist = false;
                        model.comment = db.comment.Where(s => s.tourism_id == model.activity[i].activity_id).ToList();
                        for (int j = 0; j < model.comment.Count; j++)   //找對於單個活動的所有評論內是否有含搜尋選項
                        {
                            var result = model.comment.Where(s => s.travel_month.Contains(travel_month[j])).FirstOrDefault();
                            if (result != null)
                            {
                                activity_exist = true;  //只要有一項就代表活動符合資格
                            }
                        }
                        if (!activity_exist)
                        {
                            model.activity.RemoveAt(i);     //不符合資格的移除
                            i = i - 1;
                        }
                    }
                }
            }
            return View(model);
            //return View(model.activity.OrderBy(m => m.activity_id).ToPagedList(page, pageSize));
        }



        //[ChildActionOnly]
        //public ActionResult _Hotel(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {

        //        var hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(hotel.OrderBy(m => m.hotel_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
        //[ChildActionOnly]
        //public ActionResult _Restaurant(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {
        //        var restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(restaurant.OrderBy(m => m.restaurant_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
        //[ChildActionOnly]
        //public ActionResult _Spot(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {

        //        var spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(spot.OrderBy(m => m.spot_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}