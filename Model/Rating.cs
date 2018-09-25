using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class Rating
    {
        public string Username { get; set; }

        public int Value { get; set; }

        public int MovieId { get; set; }

        public DateTime Date { get; set; }
    }
}
