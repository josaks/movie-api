using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IUserService
    {
        // Gets an authenticated user's username as a string
        string GetUserName();
    }
}
