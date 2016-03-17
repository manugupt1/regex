using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexPageProcessor
{
    class Rule
    {
        private string title;
        private string date;
        private List<Content> contents;
        private string method;
        
        public string sTitle{
            get{ return title; }
            set{ title = value;}
        }

        public string sDate
        {
            get { return date; }
            set { date = value;  }
        }

        public List<Content> sContents
        {
            get { return contents; }
            set { contents = value; }
        }

        public string sMethod
        {
            get { return method; }
            set { method = value; }
        }
    }
}
