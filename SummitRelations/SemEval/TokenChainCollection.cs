using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemEval
{
    class TokenChainCollection
    {
        public List<TokenChain> Chains { get; private set; }

        public TokenChainCollection(string tokenChain)
        {
            Chains = new List<TokenChain>();

            if (tokenChain != "_")
            {
                string[] chains = tokenChain.Split( '|' );

                foreach (string c in chains)
                    Chains.Add( new TokenChain( c ) );
            }
        }

        public void AddChain( int chainNumber, TokenChainPosition chainPosition )
        {
            if (Chains.Any(x => x.ChainNumber == chainNumber))
                OpenAndCloseChain( chainNumber, chainPosition );
            else
                Chains.Add( new TokenChain( chainNumber, chainPosition ) );
        }

        private void OpenAndCloseChain( int chainNumber, TokenChainPosition chainPosition )
        {
            TokenChain tokenChain = Chains.Single( x => x.ChainNumber == chainNumber );

            if ( tokenChain.ChainPosition == chainPosition )
                throw new Exception( "Unable to open and close chain" );

            tokenChain.OpenAndCloseChain();
        }
    }
}
