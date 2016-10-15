﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.Api.Models
{
    public class LogItem
    {
        public int Id { get; set; }
        public DateTime CaughtAt { get; set; }
        public Player CaughtBy { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
