using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexPageProcessor
{
    class RuleExtractor
    {
        private List<Rule> rules = new List<Rule>();
        List<string> fileList = new List<string>();
        private List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
        
        public RuleExtractor(string sInputFolder, string sRuleFile, string sPublisher, string sOutputFile){
            getFileList(sInputFolder);
            getRule(sRuleFile, sPublisher);
            getResults();
            saveResults(sOutputFile);
        }

        public RuleExtractor(string configFile)
        {
            string sConfig = File.ReadAllText(configFile);
            JObject jConfig = JObject.Parse(sConfig);            
            string sInputFolder, sRuleFile, sPublisher, sOutputFile;
            sInputFolder = jConfig["input_folder"].ToString();
            sRuleFile = jConfig["rule"].ToString();
            sPublisher = jConfig["publisher"].ToString();
            sOutputFile = jConfig["output"].ToString();
            getFileList(sInputFolder);
            getRule(sRuleFile, sPublisher);
            getResults();
            saveResults(sOutputFile);
        }

        public RuleExtractor(string sPublisher, string sRuleFile)
        {
            getRule(sRuleFile,sPublisher);    
        }

        private void saveResults(string sOutputFile)
        {
            Console.WriteLine("Manu : " + sOutputFile);
            StreamWriter sw = new StreamWriter(sOutputFile);
            
            foreach(Dictionary<string,string> result in results){
                sw.WriteLine(result["fileName"] + "\t" + result["title"]+"\t"+result["date"]+"\t"+ result["contents"]);
            }
            sw.Close();
        }

        private void getResults()
        {
            int pass_count=0;
            foreach (string fileName in fileList)
            {
                string contents = File.ReadAllText(fileName);
                if (getResult(fileName, contents))
                {
                    pass_count++;
                }else{
                    Console.WriteLine("****************" + fileName);
                }
            }
            Console.WriteLine(pass_count);
        }

        private bool getResult(string fileName,string fileContents)
        {
            string contents = fileContents;
            string sDate = String.Empty, sContents = String.Empty, sTitle = String.Empty;
            foreach (Rule rule in rules)
            {
                try
                {
                    // Calculate Title
                    Regex rTitleRegex = new Regex(rule.sTitle);
                    Match mTitleMatch = rTitleRegex.Match(contents);
                    Group gTitle = mTitleMatch.Groups[1];
                    sTitle = HtmlRemoval.StripTagsRegexCompiled(gTitle.Value);

                    //Calculate Date
                    Regex rDateRegex = new Regex(rule.sDate);
                    Match mDateMatch = rDateRegex.Match(contents);
                    Group gDate = mDateMatch.Groups[1];
                    sDate = HtmlRemoval.StripTagsRegexCompiled(gDate.Value);
                    //Calculate Contents
                    sContents = null;
                    foreach (Content content in rule.sContents)
                    {
                        TimeSpan ts = new TimeSpan(0, 0, 2);
                        Match mContentMatch = Regex.Match(contents, content.content, RegexOptions.None, ts);
                        for (int i = 1; i <= content.count; i++)
                        {
                            sContents = sContents + mContentMatch.Groups[i].Value + " ";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Raised:" + e.ToString());
                    continue;
                }
                sContents = HtmlRemoval.StripTagsRegexCompiled(sContents);
                sContents = sContents.Replace("\n", "");
                sContents = sContents.Replace("\r", "");
                sContents = sContents.Trim();
                if (sContents.Length != 0 && sTitle.Length != 0 && sDate.Length != 0)
                {
                    Dictionary<string, string> set = new Dictionary<string, string>();
                    set.Add("title", sTitle);
                    set.Add("date", sDate);
                    set.Add("contents", sContents);
                    set.Add("fileName", fileName);
                    results.Add(set);
                    //Console.WriteLine("----------------------------------");
                    //Console.WriteLine(fileName);
                    //Console.WriteLine(rule.sTitle + ":" + sTitle);
                    //Console.WriteLine(rule.sDate + ":" + sDate);
                    //Console.WriteLine(rule.sContents + ":" + sContents);
                    //Console.WriteLine("-----------------------------------");
                    set = null;
                    return true;
                }
                
            }
            return false;
        }


        public List<String> getResult(string fileContents)
        {
            string contents = fileContents;
            string sDate = String.Empty, sContents = String.Empty, sTitle = String.Empty;
            foreach (Rule rule in rules)
            {
                try
                {
                    // Calculate Title
                    Regex rTitleRegex = new Regex(rule.sTitle);
                    Match mTitleMatch = rTitleRegex.Match(contents);
                    Group gTitle = mTitleMatch.Groups[1];
                    sTitle = HtmlRemoval.StripTagsRegexCompiled(gTitle.Value);

                    //Calculate Date
                    Regex rDateRegex = new Regex(rule.sDate);
                    Match mDateMatch = rDateRegex.Match(contents);
                    Group gDate = mDateMatch.Groups[1];
                    sDate = HtmlRemoval.StripTagsRegexCompiled(gDate.Value);
                    //Calculate Contents
                    sContents = null;
                    foreach (Content content in rule.sContents)
                    {
                        TimeSpan ts = new TimeSpan(0, 0, 2);
                        Match mContentMatch = Regex.Match(contents, content.content, RegexOptions.None, ts);
                        for (int i = 1; i <= content.count; i++)
                        {
                            sContents = sContents + mContentMatch.Groups[i].Value + " ";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Raised:" + e.ToString());
                    continue;
                }
                sContents = HtmlRemoval.StripTagsRegexCompiled(sContents);
                sContents = sContents.Replace("\n", "");
                sContents = sContents.Replace("\r", "");
                sContents = sContents.Trim();
                if (sContents.Length != 0 && sTitle.Length != 0 && sDate.Length != 0)
                {
                    List<String> set = new List<string>();
                    set.Add(sTitle);
                    set.Add(sDate);
                    set.Add(sContents);
                    //Console.WriteLine("----------------------------------");
                    //Console.WriteLine(fileName);
                    //Console.WriteLine(rule.sTitle + ":" + sTitle);
                    //Console.WriteLine(rule.sDate + ":" + sDate);
                    //Console.WriteLine(rule.sContents + ":" + sContents);
                    //Console.WriteLine("-----------------------------------");
                    return set;
                }

            }
            return null;
        }



        private void getRule(string sRuleFile, string sPublisher)
        {
            string sRuleText = File.ReadAllText(sRuleFile);
            JObject jRules = JObject.Parse(sRuleText);
         
            foreach (var rule in jRules)
            {
                if (rule.Key == sPublisher)
                {
                    foreach (var set in rule.Value)
                    {
                        Rule rRule = new Rule();
                        List<Content> lContent = new List<Content>();
                        rRule.sTitle = set["title"].ToString();
                        rRule.sDate = set["date"].ToString();
                        rRule.sMethod = set["method"].ToString();
                        foreach (var content in set["content"])
                        {
                            Content cContent = new Content();
                            cContent.content = content["rule"].ToString();
                            cContent.count = int.Parse(content["count"].ToString());
                            lContent.Add(cContent);
                            cContent = null;
                        }
                        rRule.sContents = lContent;
                        rules.Add(rRule);     
                    }
                    break;
                }
            }
        }

        private void getFileList(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                fileList.Add(file);
            }
        }
    }
}
