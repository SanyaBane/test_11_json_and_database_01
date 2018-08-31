using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_11_json_and_database_01
{
    public class JsonFile_1
    {
        public class LockInfo
        {
            public string Info { get; set; }
        }

        public class Lock
        {
            public bool LockSuccess { get; set; }
            public bool ForceLock { get; set; }
            public string LockId { get; set; }
            public LockInfo LockInfo { get; set; }
        }

        public class Price
        {
            public int id { get; set; }
            public int cost { get; set; }
            public int list { get; set; }
            public int retail { get; set; }
            public int purchase { get; set; }
            public int reduction { get; set; }
            public string valid_from { get; set; }
            public int tax_code_id { get; set; }
            public int sales_channel_id { get; set; }
        }

        public class Values
        {
            public string name { get; set; }
            public string comment { get; set; }
            public int language_id { get; set; }
        }

        public class Attribute
        {
            public int ord { get; set; }
            public List<Note> notes { get; set; }
            public object value { get; set; }
            public Values values { get; set; }
            public int? value_id { get; set; }
            public int is_public { get; set; }
            public int attribute_id { get; set; }
        }

        public class Note
        {
            public string txt { get; set; }
            public int language_id { get; set; }
        }

        public class Data
        {
            public List<Price> prices { get; set; }
            public int article_id { get; set; }
            public List<Attribute> attributes { get; set; }
            public int consingment { get; set; }
            public long article_type_id { get; set; }
        }

        public class RootObject
        {
            public Lock Lock { get; set; }
            public object Paging { get; set; }
            public Data Data { get; set; }
            public object Info { get; set; }
        }
    }
}
