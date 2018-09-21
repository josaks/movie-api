using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Persistence
{
    public interface ICache
    {
        List<Movie> GetAllMovies();
        Movie GetMovie(int id);
        Movie AddComment(Comment comment);
    }
}
