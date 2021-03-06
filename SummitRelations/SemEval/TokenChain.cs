﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemEval
{
    enum TokenChainPosition
    {
        Open,
        Close,
        OpenAndClose
    }

    class TokenChain
    {
        public int ChainNumber { get; private set; }
        public TokenChainPosition ChainPosition { get; private set; }

        public TokenChain(string chain)
        {
            string regex;
            if (chain.StartsWith("("))
            {
                if (chain.EndsWith(")"))
                {
                    ChainPosition = TokenChainPosition.OpenAndClose;
                    regex = @"\((\d+)\)";
                }
                else
                {
                    ChainPosition = TokenChainPosition.Open;
                    regex = @"\((\d+)";
                }
            }
            else
            {
                ChainPosition = TokenChainPosition.Close;
                regex = @"(\d+)\)";
            }
            Match match = new Regex(regex).Match( chain );
            this.ChainNumber = int.Parse( match.Groups[ 1 ].ToString() );
        }

        public TokenChain( int chainNumber, TokenChainPosition chainPosition )
        {
            this.ChainNumber = chainNumber;
            this.ChainPosition = chainPosition;
        }

        public void OpenAndCloseChain()
        {
            ChainPosition = TokenChainPosition.OpenAndClose;
        }

        public string GenerateSemEval()
        {
            switch ( ChainPosition )
            {
                case TokenChainPosition.Open:
                    return "(" + ChainNumber;
                case TokenChainPosition.Close:
                    return ChainNumber + ")";
                case TokenChainPosition.OpenAndClose:
                    return "(" + ChainNumber + ")";
                default:
                    throw new Exception();
            }
        }
    }
}
