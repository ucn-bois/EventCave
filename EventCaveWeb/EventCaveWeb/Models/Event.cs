﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventCaveWeb.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime Datetime { get; set; }
        [Required]
        public bool Public { get; set; }
        [Required]
        public int Limit { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        // TODO: Allow later
        // [Required]
        // public ApplicationUser Host { get; set; }
    }
}