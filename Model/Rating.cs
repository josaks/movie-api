using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel
{
    public class Rating
    {
        public string Username { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public int MovieId { get; set; }
        
        public DateTime Date { get; set; }
    }
}
