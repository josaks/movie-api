using System;
using System.Collections.Generic;
using System.Text;

namespace Service {
    public class DateService : IDateService {
        public DateTime Now() => DateTime.Now;
    }
}
