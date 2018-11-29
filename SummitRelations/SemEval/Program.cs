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
            string[] lines = File.ReadAllLines( @"SemEvalOriginal\SummitSemEvalDoisApostos.txt", Encoding.GetEncoding( "iso-8859-1" ) );
            SemEval semEval = new SemEval( lines );
            semEval.RemoveQuotesLines();
            string[] newFile = semEval.GenerateSemEval();
            File.WriteAllText( "SemEval_" + DateTime.Now.ToString( "yyyymmdd_hhMMss" ), String.Join(Environment.NewLine, newFile ) );

            // TODO: fazer compare entre o original e o output
        }
    }
}
