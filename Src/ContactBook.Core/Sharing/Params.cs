using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Sharing
{
    public class Params
    {
        public int MaxPageSize { get; set; } = 15;
        private int _pageSize = 3;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;
        private string _search;

        public string? Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }
    }
}
