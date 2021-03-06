﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModels.EF {
    public class Rating
    {
        public string Username { get; set; }
        
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int RatingValue { get; set; }

        public DateTime Date { get; set; }
    }
}
