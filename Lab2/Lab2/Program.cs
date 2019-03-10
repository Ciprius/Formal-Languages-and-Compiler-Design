using Lab2.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static Cont controller = new Cont();
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("1: read from keyboard");
                Console.WriteLine("2: read from file");
                Console.WriteLine("0: exit");
                Console.Write("Input:");
                string option = Console.ReadLine();
                if (option.Equals("0"))
                    break;
                if (option.Equals("1"))
                    KeyboardMenu();
                if (option.Equals("2"))
                    FileMenu();
            } 
        }

        private static void KeyboardMenu()
        {
            bool prog = true;
            while (prog)
            {
                Console.WriteLine("1: Grammar");
                Console.WriteLine("2: FiniteAutomata");
                Console.WriteLine("0: exit");
                Console.Write("Input:");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        GrammarMenu(1);
                        break;
                    case "2":
                        FiniteAutomataMenu(1);
                        break;
                    case "0":
                        prog = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void FileMenu()
        {
            bool prog = true;
            while (prog)
            {
                Console.WriteLine("1: Grammar");
                Console.WriteLine("2: FiniteAutomata");
                Console.WriteLine("0: exit");
                Console.Write("Input:");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        GrammarMenu(2);
                        break;
                    case "2":
                        FiniteAutomataMenu(2);
                        break;
                    case "0":
                        prog = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void GrammarMenu(int code)
        {
            if (code.Equals(1))
                controller.ReadFromKeyBoardGrammar();
            else
                controller.ReadFromFileGrammar();

            try
            {
                controller.CheckGrm();
                bool prog = true;
                while (prog)
                {
                    Console.WriteLine("1: Show non-terminals");
                    Console.WriteLine("2: Show terminals");
                    Console.WriteLine("3: Show products");
                    Console.WriteLine("4: Show products for a given non-terminal");
                    Console.WriteLine("5: Show grammar");
                    Console.WriteLine("6: Check if the grammar if regular");
                    Console.WriteLine("7: Convert into Finite Automata");
                    Console.WriteLine("0: exit");
                    Console.Write("Input:");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "0":
                            prog = false;
                            break;
                        case "1":
                            controller.ShowNonterminals();
                            Console.WriteLine();
                            break;
                        case "2":
                            controller.ShowTerminals();
                            Console.WriteLine();
                            break;
                        case "3":
                            controller.ShowProducts();
                            Console.WriteLine();
                            break;
                        case "4":
                            controller.ShowNonterminals();
                            Console.Write("Choose one:");
                            string input = Console.ReadLine();
                            controller.ShowProduct(input);
                            break;
                        case "5":
                            controller.ShowGrammar();
                            break;
                        case "6":
                            controller.CheckGrammar();
                            Console.WriteLine();
                            break;
                        case "7":
                            bool res = controller.TryConvertToFinite();
                            if (!res)
                                Console.WriteLine("The grammar is not regular, thus cannot convert to Finite Automata");
                            else
                                controller.ShowFiniteAutomata();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message.ToString());
                Console.Read();
            }
        }

        private static void FiniteAutomataMenu(int code)
        {
            if (code.Equals(1))
                controller.ReadFromKeyBoardFiniteAutomata();
            else
                controller.ReadFromFileFiniteAutomata();

            try
            {
                controller.CheckFntAuto();
                bool prog = true;
                while (prog)
                {
                    Console.WriteLine("1: Show set of finite states");
                    Console.WriteLine("2: Show alphabet");
                    Console.WriteLine("3: Show transitions");
                    Console.WriteLine("4: Show final states");
                    Console.WriteLine("5: Show finite automata ");
                    Console.WriteLine("6: Convert into Grammar");
                    Console.WriteLine("0: exit");
                    Console.Write("Input:");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "0":
                            prog = false;
                            break;
                        case "1":
                            controller.ShowFiniteSetOfState();
                            Console.WriteLine();
                            break;
                        case "2":
                            controller.ShowAlphabet();
                            Console.WriteLine();
                            break;
                        case "3":
                            controller.ShowLambda();
                            Console.WriteLine();
                            break;
                        case "4":
                            controller.ShowFinalStates();
                            break;
                        case "5":
                            controller.ShowFiniteAutomata();
                            break;
                        case "6":
                            controller.ConvertToGrammar();
                            controller.ShowGrammar();
                            Console.WriteLine();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message.ToString());
                Console.Read();
            }
        }
    }
}
