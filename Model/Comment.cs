using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public int MovieId { get; set; }
    }
}
