using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemEval
{
    class Program
    {
        static void Main( string[] args )
        {
            Encoding iso = Encoding.GetEncoding( "iso-8859-1" );
            string[] lines = File.ReadAllLines( @"SemEvalOriginal\SummitSemEvalDoisApostos.txt", iso );
            SemEval semEval = new SemEval( lines );
            semEval.RemoveQuotesLines();
            string newFile = semEval.GenerateSemEval();
            File.WriteAllText( "SemEval_" + DateTime.Now.ToString( "yyyymmdd_hhMMss" ) + ".txt", newFile , iso );
        }
    }
}
