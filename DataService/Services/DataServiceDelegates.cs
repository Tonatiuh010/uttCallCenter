﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class DataServiceDelegates
    {
        public delegate void DataException(Exception e, string customMsg = "");
    }
}
