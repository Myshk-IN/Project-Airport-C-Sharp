using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka_API
{
    class Books
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public ushort ReleaseDate { get; set; }
        public string Publisher { get; set; }
        public ushort Pages { get; set; }
        public string ISBN { get; set; }
    }
}
