using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemEval
{
    class Sentence
    {
        private List<Token> tokens;

        public Sentence(string[] lines)
        {
            this.tokens = new List<Token>();
            foreach ( string l in lines )
                tokens.Add( new Token( l ) );
        }

        public string GenerateSemEval()
        {
            string semEval = "";
            int lineCount = 0;

            foreach (Token t in tokens)
            {
                semEval += lineCount + "\t" + t.GenerateSemEval() + "\n";
                lineCount++;
            }

            semEval += "\n";

            return semEval;
        }

        public void RemoveQuotesLines()
        {
            List<Token> newTokens = new List<Token>();

            for (int i=0; i<tokens.Count; i++)
            {
                Token t = tokens[ i ];
                if (t.Word == "\"")
                {
                    TokenChainCollection chains = t.Chains;
                    foreach (TokenChain c in chains.Chains)
                    {
                        if ( c.ChainPosition == TokenChainPosition.Close )
                        {
                            tokens[ i - 1 ].AddTokenChain( c.ChainNumber, c.ChainPosition ); // move to previous token
                        }
                        else if ( c.ChainPosition == TokenChainPosition.Open )
                        {
                            tokens[ i + 1 ].AddTokenChain( c.ChainNumber, c.ChainPosition ); // move to next token
                        }
                        else
                            throw new Exception( "Unexpected chain position" );
                    }
                }
                else
                {
                    newTokens.Add( t );
                }
            }

            tokens = newTokens;
        }
    }
}
