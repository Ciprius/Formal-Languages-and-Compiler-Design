using Lab2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Controller
{
    class Cont
    {
        private Grammar grammar;
        private FiniteAutomata finite;

        public Cont(){ }

        public void CheckGrm() { this.grammar.Check(); }
        public void CheckFntAuto() { this.finite.Check(); }

        public void ReadFromKeyBoardGrammar()
        {
            List<string> P = new List<string>();
            Console.WriteLine("Give the set of non-terminals, separated by comma:");
            Console.Write("N = S");
            string strN = "S" + Console.ReadLine();
            Console.WriteLine("Give the set of terminals, separated by comma:");
            Console.Write("Epsilon = ");
            string strEpsilon = Console.ReadLine();
            Console.WriteLine("Give the set of production:");
            Console.Write("S->");
            string strP = "S->" + Console.ReadLine();
            P.Add(strP);

            while (true)
            {
                string str = Console.ReadLine();
                if (str.Equals("done"))
                    break;
                P.Add(str);
            }
            this.grammar = new Grammar(Convert(strN.Split(',')), Convert(strEpsilon.Split(',')), P);
        }

        public void ReadFromKeyBoardFiniteAutomata()
        {
            List<string> lambda = new List<string>();
            Console.WriteLine("Give the set of finite states separated by comma:");
            Console.Write("Q = ");
            string strQ = Console.ReadLine();
            Console.WriteLine("Give the alphabet, separated by comma:");
            Console.Write("Epsilon = ");
            string strEpsilon = Console.ReadLine();
            Console.WriteLine("Give the starting state:");
            Console.Write("q0 = ");
            string q0 = Console.ReadLine();
            Console.WriteLine("Give the final states:");
            Console.Write("F = ");
            string strF = Console.ReadLine();
            Console.WriteLine("Give the set of transitions:");

            while(true)
            {
                string str = Console.ReadLine();
                if (str.Equals("done"))
                    break;
                lambda.Add(str);
            }
            this.finite = new FiniteAutomata(Convert(strQ.Split(',')), Convert(strEpsilon.Split(',')), Convert(strF.Split(',')), q0, lambda);
        }

        public void ReadFromFileGrammar()
        {
            List<string> P = new List<string>();
            string lineP;
            string lineN = "";
            string lineEpsilon = "";
            System.IO.StreamReader file = new System.IO.StreamReader("D:\\Faculta\\An III\\Semestru_1\\LFTC\\Lab2\\grammar.txt");
            lineN = file.ReadLine();
            lineEpsilon = file.ReadLine();
            while ((lineP = file.ReadLine()) != null)
                P.Add(lineP);
            this.grammar = new Grammar(Convert(lineN.Split(',')),Convert(lineEpsilon.Split(',')),P);
            file.Close();
        }

        public void ReadFromFileFiniteAutomata()
        {
            List<string> lambda = new List<string>();
            string lineQ;
            string lineEpsilon;
            string lineF;
            string q0;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("D:\\Faculta\\An III\\Semestru_1\\LFTC\\Lab2\\finiteautomata.txt");
            lineQ = file.ReadLine();
            lineEpsilon = file.ReadLine();
            q0 = file.ReadLine();
            lineF = file.ReadLine();
            while ((line = file.ReadLine()) != null)
                lambda.Add(line);
            this.finite = new FiniteAutomata(Convert(lineQ.Split(',')),Convert(lineEpsilon.Split(',')),Convert(lineF.Split(',')),q0,lambda);
            file.Close();
        }

        private List<string> Convert(string[] str)
        {
            List<string> strs = new List<string>();

            for (int i = 0; i < str.Length; i++)
                strs.Add(str[i]);
            return strs;
        }

        public void CheckGrammar()
        {
            if (this.grammar.CheckIfRegular())
                Console.WriteLine("The grammar is regular");
            else
                Console.WriteLine("The gramar is not regular");
        }

        public void ShowGrammar()
        {
            Console.WriteLine(this.grammar.GetHeader());
            ShowNonterminals();
            ShowTerminals();
            ShowProducts();
            Console.WriteLine("S:S");
            Console.WriteLine();
        }

        public void ShowNonterminals()
        {
            List<string> items = this.grammar.GetSetOfNonTerminals();
            Console.Write("N = {");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}{1}", items[i], (i + 1) == items.Count ? "}" : ",");
            }
            Console.WriteLine();
        }

        public void ShowTerminals()
        {
            List<string> items = this.grammar.GetSetOfTerminals();
            Console.Write("Epsilon = {");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}{1}", items[i], (i + 1) == items.Count ? "}" : ",");
            }
            Console.WriteLine();
        }

        public void ShowProducts()
        {
            List<string> items = this.grammar.GetSetOfProductions();
            int ok = 0;
            foreach (string item in items)
            {
                if (ok == 0)
                {
                    Console.WriteLine("P: " + item);
                    ok = 1;
                }
                else
                    Console.WriteLine("   " + item);
            }
        }

        public void ShowProduct(string input)
        {
            List<string> items = this.grammar.GetSetOfProductions();

            Console.WriteLine("You choose {0}",input);
            foreach(string item in items)
            {
                string[] itemSplit = item.Split('-');
                if (input.Equals(itemSplit[0]))
                    Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        public bool TryConvertToFinite()
        {
            if (!this.grammar.CheckIfRegular())
                return false;

            this.finite = new FiniteAutomata();
            finite.SetInitialState("S");
            finite.SetAlphabet(Copy(this.grammar.GetSetOfTerminals()));
            finite.SetFiniteSetOfStates(Copy(this.grammar.GetSetOfNonTerminals()));

            int ok = 1;
            foreach (string item in this.grammar.GetSetOfProductions())
            {
                string[] itemSplit = item.Split(new string[] { "->" }, StringSplitOptions.None);
                string[] RHSSplit = itemSplit[1].Split('|');
                
                for (int i = 0; i < RHSSplit.Length; i++)
                {
                    char[] chars = RHSSplit[i].ToCharArray();
                    string str = this.grammar.ComposeString(chars);
                    if (itemSplit[0].Equals('S') && str.Equals("$"))
                        finite.SetFinalState("S");
                    if (str.Equals("") && ok.Equals(1))
                    {
                        finite.AddK();
                        ok = 0;
                    }
                    if (str.Equals(""))
                    {
                        finite.AddToLambda(itemSplit[0] + "," + chars[0].ToString() + "=" + "&");
                    }
                    else
                    {
                        finite.AddToLambda(itemSplit[0] + "," + chars[0].ToString() + "=" + str);
                    }
                }
            }
            return true;
        }

        private List<String> Copy(List<string> items)
        {
            List<string> newlist = new List<string>();
            foreach (string item in items)
                    newlist.Add(item);
            return newlist;
        }

        public void ShowFiniteAutomata()
        {
            Console.WriteLine(this.finite.GetHeader());
            ShowFiniteSetOfState();
            ShowAlphabet();
            Console.WriteLine("q0 = " + this.finite.GetInitialState());
            ShowFinalStates();
            ShowLambda();
        }

        public void ShowFiniteSetOfState()
        {
            List<string> items = this.finite.GetFiniteSetOfStates();
            Console.Write("Q = {");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}{1}", items[i], (i + 1) == items.Count ? "}" : ",");
            }
            Console.WriteLine();
        }

        public void ShowAlphabet()
        {
            List<string> items = this.finite.GetAlphabet();
            Console.Write("Epsilon = {");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}{1}", items[i], (i + 1) == items.Count ? "}" : ",");
            }
            Console.WriteLine();
        }

        public void ShowFinalStates()
        {
            List<string> items = this.finite.GetFinalStates();
            Console.Write("F = {");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}{1}", items[i], (i + 1) == items.Count ? "}" : ",");
            }
            Console.WriteLine();
        }

        public void ShowLambda()
        {
            Console.WriteLine("Lambda:");
            foreach(string item in this.finite.GetLambda())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        public void ConvertToGrammar()
        {
            this.grammar = new Grammar();
            this.grammar.SetNonTerminals(Copy(this.finite.GetFiniteSetOfStates()));
            this.grammar.SetTerminals(Copy(this.finite.GetAlphabet()));
            List<string> products = new List<string>();

            foreach (string item in this.finite.GetLambda())
            {
                string[] itemSplit = item.Split('=');
                string[] LHSSplit = itemSplit[0].Split(',');
                if (itemSplit[1].Equals("&"))
                    products.Add(LHSSplit[0] + "->" + LHSSplit[1]);
                products.Add(LHSSplit[0] + "->" + LHSSplit[1] + itemSplit[1]);
            }

            if (this.finite.CheckInitState())
                products.Add(this.finite.GetInitialState() + "->$");
            this.grammar.SetProductions(products);
        }
    }
}
