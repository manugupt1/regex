using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegexPageProcessor
{
    class Engine
    {
        string [,] args;
        public Engine()
        {
            args = new string[,]
            {
                {"nbc_test.html","nbcnews.com","rules1.json"},
                {"huff_test.html","huffingtonpost.com","rules1.json"},
                {"ny_test.html","nytimes.com","rules1.json"},
            };

            for (int i = 0; i < args.GetLength(0); i++)
            {
                Console.Write(args[i, 0] + "\t" + args[i, 1] + "\t" + args[i,2]);
                RuleExtractor RuleEx = new RuleExtractor(args[i,1], args[i,2]);
                string contents = File.ReadAllText(args[i,0]);
                List<string> list = RuleEx.getResult(contents);
                foreach (var val in list)
                {
                    Console.Write(val + "\t");
                }
                Console.WriteLine("---");
            }
        } 
    }
}
