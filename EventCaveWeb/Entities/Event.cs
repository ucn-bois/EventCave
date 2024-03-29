﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventCaveWeb.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Images { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime Datetime { get; set; }

        public string Description { get; set; }

        public int Limit { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public virtual ApplicationUser Host { get; set; }

        public virtual ICollection<UserEvent> Attendees { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}