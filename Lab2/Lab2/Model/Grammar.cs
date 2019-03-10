using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2.Model
{
    class Grammar
    {
        private string G = "G=(N,Epsilon,P,S)";
        private List<string> N;
        private List<string> Epsilon;
        private List<string> P;
        private string Upper = @"^[A-Z]$";
        private string Lower = @"^[a-z0-9]$";
        private int flag;

        public Grammar(List<string> N, List<string> Epsilon, List<string> P)
        {
            this.N = N;
            this.Epsilon = Epsilon;
            this.P = P;
            this.flag = 1;
        }

        public Grammar()
        {
            this.N = new List<string>();
            this.Epsilon = new List<string>();
            this.P = new List<string>();
        }

        //getters 
        public string GetHeader() { return this.G; }
        public List<string> GetSetOfNonTerminals() { return this.N; }
        public List<string> GetSetOfTerminals() { return this.Epsilon; }
        public List<string> GetSetOfProductions() { return this.P; }

        //setters
        public void SetNonTerminals(List<string> N) { this.N = N; }
        public void SetTerminals(List<string> Epsilon) { this.Epsilon = Epsilon; }
        public void SetProductions(List<string> P) { this.P = P; }

        public bool CheckIfRegular()
        {
            Regex regex = new Regex(this.Lower);
            foreach(string item in this.P)
            {
                string[] itemSplit = item.Split(new string[] {"->"}, StringSplitOptions.None);
                string[] RHSSplit = itemSplit[1].Split('|');
                for (int i = 0; i< RHSSplit.Length; i++)
                {
                    char[] chars = RHSSplit[i].ToCharArray();
                    if (!regex.IsMatch(chars[0].ToString()))
                        return false;
                    if (!itemSplit[0].Equals("S") && ComposeString(chars).Equals("$"))
                        return false;
                    if (itemSplit[0].Equals("S") && ComposeString(chars).Equals("$"))
                        this.flag = 0;
                    if (ComposeString(chars).Equals("S") && flag == 0)
                        return false;
                }
            }
            return true;
        }

        public string ComposeString(char[] chars)
        {
            string str = "";
            for (int i = 1; i < chars.Length; i++)
            {
                str += chars[i];
            }
            return str;
        }

        public void Check()
        {
            string err = "";
            if (!CheckNonTerminals(Upper))
                err +="Non-terminals must be capital letters" + "\n";
            if (!(CheckTerminals(Lower)))
                err += "Terminals must be small letters/digits" + "\n";

            if (!err.Equals(""))
                throw new Exception(err);
        }

        private bool CheckNonTerminals(string pattern)
        {
            Regex regex = new Regex(pattern);
            foreach (string item in this.N)
            {
                if (!regex.IsMatch(item))
                    return false;
            }
            return true;
        }

        private bool CheckTerminals(string pattern)
        {
            Regex regex = new Regex(pattern);
            foreach (string item in this.Epsilon)
            {
                if (!regex.IsMatch(item))
                    return false;
            }
            return true;
        }
    }
}
