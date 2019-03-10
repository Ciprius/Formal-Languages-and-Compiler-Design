using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2.Model
{
    class FiniteAutomata
    {
        private string M = "M=(Q,Epsilon,Lambda,q0,F)";
        private List<string> lambda;
        private List<string> Q;
        private List<string> Epsilon;
        private List<string> F;
        private string q0;
        private string Upper = @"^[A-Z&]$";
        private string Lower = @"^[a-z0-9]$";

        public FiniteAutomata()
        {
            this.Epsilon = new List<string>();
            this.F = new List<string>();
            this.Q = new List<string>();
            this.lambda = new List<string>();
        }

        public FiniteAutomata(List<string> Q, List<string> Epsilon, List<string> F, string q0, List<string> lambda)
        {
            this.Q = Q;
            this.Epsilon = Epsilon;
            this.F = F;
            this.lambda = lambda;
            this.q0 = q0;
        }

        //getters
        public string GetHeader() { return this.M; }
        public List<string> GetFiniteSetOfStates() { return this.Q; }
        public List<string> GetAlphabet() { return this.Epsilon; }
        public List<string> GetFinalStates() { return this.F; }
        public string GetInitialState() { return this.q0; }
        public List<string> GetLambda() { return this.lambda; }

        //setters
        public void SetFiniteSetOfStates(List<string> Q) { this.Q = Q; }
        public void SetAlphabet(List<string> Epsilon) { this.Epsilon = Epsilon; }
        public void SetInitialState(string q0) { this.q0 = q0; }
        public void SetFinalState(string final) { this.F.Add(final); }
        public void AddK() { this.Q.Add("&"); this.F.Add("&"); }
        public void AddToLambda(string val) { this.lambda.Add(val); }

        public void Check()
        {
            string err = "";
            if (!CheckSetofFiniteState(Upper))
                err += "The set of states must be capital letters" + "\n";
            if (!(CheckTransitions(Lower)))
                err += "The alphabet must be small letters/digits" + "\n";

            if (!err.Equals(""))
                throw new Exception(err);
        }

        private bool CheckSetofFiniteState(string pattern)
        {
            Regex regex = new Regex(pattern);
            foreach (string item in this.Q)
            {
                if (!regex.IsMatch(item))
                    return false;
            }
            return true;
        }

        private bool CheckTransitions(string pattern)
        {
            Regex regex = new Regex(pattern);
            foreach (string item in this.Epsilon)
            {
                if (!regex.IsMatch(item))
                    return false;
            }
            return true;
        }

        public bool CheckInitState()
        {
            foreach (string item in this.F)
                if (item.Equals(this.q0))
                    return true;
            return false;
        }
    }
}
