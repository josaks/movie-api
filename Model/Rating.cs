using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel
{
    public class Rating
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
