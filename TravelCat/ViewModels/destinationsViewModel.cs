﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;
using X.PagedList.Mvc;
using X.PagedList;

namespace TravelCat.ViewModels
{
    public class destinationsViewModel
    {
        public restaurant restaurant { get; set; }
        public hotel hotel { get; set; }
        public activity activity { get; set; }
        public spot spot { get; set; }
        public List<restaurant> restaurant_list { get; set; }
        public List<hotel> hotel_list { get; set; }
        public List<activity> activity_list { get; set; }
        public List<spot> spot_list { get; set; }
        public List<comment> comment { get; set; }
        public List<message> message { get; set; }
        public List<member_profile> member_profile { get; set; }
        public List<member> member { get; set; }
        public List<message_emoji_details> message_emoji_details { get; set; }
        public List<comment_emoji_details> comment_emoji_details { get; set; }
        public List<emoji> emoji { get; set; }
        public List<collections_detail> collections_detail { get; set; }
        public IPagedList<comment> comment_page { get; set; }
    }
}