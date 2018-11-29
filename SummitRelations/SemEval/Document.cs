using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemEval
{
    class Document
    {
        private string name;
        private List<Sentence> sentences;

        public Document(string name, string[] lines)
        {
            this.name = name;
            this.sentences = new List<Sentence>();

            List<string[]> linesBySentence = BreakBySentence( lines );
            foreach (string[] sentenceLines in linesBySentence)
            {
                this.sentences.Add( new Sentence( sentenceLines ) );
            }
        }

        public void RemoveQuotesLines()
        {
            foreach (Sentence s in sentences)
                s.RemoveQuotesLines();
        }

        public string GenerateSemEval()
        {
            string semEval = "#begin document " + name + "\n";

            foreach (Sentence s in sentences)
                semEval += s.GenerateSemEval();

            semEval += "#end document" + "\n";

            return semEval;
        }

        private List<string[]> BreakBySentence(string[] document)
        {
            List<string[]> sentences = new List<string[]>();
            List<string> sentenceLines = new List<string>();
            foreach (string l in document)
            {
                if ( string.IsNullOrEmpty( l.Trim() ) )
                {
                    sentences.Add( sentenceLines.ToArray() );
                    sentenceLines = new List<string>();
                }
                else
                {
                    sentenceLines.Add( l );
                }
            }

            if (sentenceLines.Count > 0)
            {
                sentences.Add( sentenceLines.ToArray() );
            }

            return sentences;
        }
    }
}
