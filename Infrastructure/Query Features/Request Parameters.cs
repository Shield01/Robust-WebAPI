using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Query_Features
{
    public abstract class Request_Parameters
    {
        const int maxPage = 50;

        public int pageNumber {get; set;} = 1;

        private int _pageSize = 2;

        public int pageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPage) ? maxPage : value;
            }
        }
    }
}
