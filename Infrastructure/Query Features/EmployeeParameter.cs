using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Query_Features
{
    public class EmployeeParameter : Request_Parameters
    {
        public uint MinAge { get; set; }

        public uint MaxAge { get; set; } = int.MaxValue;

        public bool ValidAgeRange => MaxAge > MinAge;
    }
}
