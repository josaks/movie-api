using System;
using System.Collections.Generic;
using System.Text;

namespace Service {
    public interface IDateService {
        // Get a datetime object representing the current date and time
        DateTime Now();
    }
}
