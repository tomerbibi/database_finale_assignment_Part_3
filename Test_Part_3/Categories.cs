using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Part_3
{
    class Categories
    {
        public Categories()
        {
        }

        public Categories(long iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public long ID { get; set; }
        public string Name { get; set; }
    }
}
