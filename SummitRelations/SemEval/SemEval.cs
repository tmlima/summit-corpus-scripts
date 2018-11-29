using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemEval
{
    class SemEval
    {
        List<Document> documents;

        public SemEval(string[] file)
        {
            this.documents = new List<Document>();

            string currentDocument = "";
            List<string> currentDocumentLines = new List<string>();
            foreach (string line in file)
            {
                if (line.Contains("#begin document"))
                {
                    if ( !string.IsNullOrEmpty( currentDocument ) )
                        throw new Exception( "Document " + currentDocument + " not finished" );

                    currentDocument = ParseDocumentName(line);
                }
                else if (line.Contains( "#end document"))
                {
                    documents.Add( new Document( currentDocument, currentDocumentLines.ToArray() ) );
                    currentDocumentLines = new List<string>();
                    currentDocument = "";
                }
                else
                {
                    currentDocumentLines.Add( line );
                }
            }

            if ( !string.IsNullOrEmpty( currentDocument ) )
                throw new Exception( "Document " + currentDocument + " not finished" );
        }

        public void RemoveQuotesLines()
        {
            foreach (Document d in documents)
                d.RemoveQuotesLines();
        }

        public string[] GenerateSemEval()
        {
            throw new NotImplementedException();
        }

        private string ParseDocumentName(string line)
        {
            Regex regex = new Regex( @"#begin document (\S+)" );
            Match match = regex.Match( line );
            return match.Groups[ 1 ].Value;
        }
    }
}
