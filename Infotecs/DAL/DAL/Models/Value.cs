﻿namespace Infotecs.DAL.Models
{
    public class Value
    {
        public int id { get; set; }
        public DateTime datetime { get; set; }
        public int timeinseconds { get; set; }
        public double indicatorvalue { get; set; }
        public required string filename { get; set; }
    }
}
