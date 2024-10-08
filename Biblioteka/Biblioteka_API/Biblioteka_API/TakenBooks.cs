using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka_API
{
    class TakenBooks
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime TakenFrom { get; set; }
        public DateTime TakenUntil { get; set; }
        public byte Qnt { get; set; } //sbyte byte, but with negative numbers
    }
}
