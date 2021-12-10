using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Query_Features
{
     public class CompanyParameter : Request_Parameters
    {
        public string SearchTerm { get; set; }
    }
}
