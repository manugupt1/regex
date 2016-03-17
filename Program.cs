using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexPageProcessor
{
    class Program
    {

        static List<Dictionary<string, string>> results;
        static List<Dictionary<string, string>> rules;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Using config file, for cmd parameters use the following");
                Console.WriteLine("RegexPatternFinder <inputfolder_path> <rulesfile_path> <publisher_name> <outputfile_path_name>");
                new RuleExtractor("config.json");
            }else if (args.Length == 4)
            {
                string inputFolder = args[0];
                string rulesFile = args[1];
                string publisher = args[2];
                string outputFile = args[3];
                new RuleExtractor(inputFolder, rulesFile, publisher, outputFile);
            }else if(args[0]=="engine"){
                Console.WriteLine("In here"); 
                new Engine();
            }
        }
    }

}