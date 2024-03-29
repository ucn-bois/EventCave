﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventCaveWeb.Entities
{
    public class UserEvent
    {
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }
        [Key, Column(Order = 2)]
        public int EventId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Event Event { get; set; }
    }
}