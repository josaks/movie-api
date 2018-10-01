using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Text { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int MovieId { get; set; }

        public DateTime Date { get; set; }
    }
}
