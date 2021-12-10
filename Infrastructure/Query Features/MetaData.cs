using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Query_Features
{
    public class MetaData
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool hasPrevious => CurrentPage > 1;

        public bool hasNextPage => CurrentPage < TotalPages;
    }
}
