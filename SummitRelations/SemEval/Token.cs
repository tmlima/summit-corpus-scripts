using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemEval
{
    class Token
    {
        private string[] tokens;
        public string Word { get { return tokens[ 1 ]; } }
        public TokenChainCollection Chains { get; private set; }

        public Token(string line)
        {
            tokens = new string[ 8 ];
            string[] columns = line.Split( '\t' );
            Array.Copy(columns, this.tokens, 8);
            Chains = new TokenChainCollection(columns[8]);
        }

        public void AddTokenChain(int chainNumber, TokenChainPosition chainPosition)
        {
            Chains.AddChain( chainNumber, chainPosition );
        }
    }
}
