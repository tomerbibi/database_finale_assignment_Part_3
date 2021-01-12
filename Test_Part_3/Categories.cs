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
        public override bool Equals(object obj)
        {
            return this.ID == ((Store)obj).ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }
        public static bool operator ==(Categories category1, Categories category2)
        {
            return category1.ID == category2.ID;
        }
        public static bool operator !=(Categories category1, Categories category2)
        {
            return !(category1 == category2);
        }


        public override string ToString()
        {
            return $"{Newtonsoft.Json.JsonConvert.SerializeObject(this)}";
        }
    }
}
