using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaTruTraveller.Models
{
    public class Post
    {
        // SQLite Post
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }

        //[MaxLength(250)]
        //public string Description { get; set; }

        //public string VenueName { get; set; }
        //public string CategoryId { get; set; }
        //public string CategoryName { get; set; }
        //public string Address { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }
        //public int Distance { get; set; }
        //public string UserId { get; set; }

        // Firestore Post
        public string Id { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
        public string VenueName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Distance { get; set; }
        public string UserId { get; set; }
    }
}
