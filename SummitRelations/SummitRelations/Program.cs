using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummitRelations
{
    class Program
    {
        const string Indirect = "indirect";

        static void Main(string[] args)
        {
            string textsFolder = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Texts";
            RelationsExtractor extractor = new RelationsExtractor(textsFolder);

            List<Pos> pos = extractor.GetPos();

            List<Markable> markables = extractor.GetMarkables();

            ElementosDaCadeiaAnterioresIndiretos(pos, markables);

            IndiretosPlural(pos, markables);

            Console.WriteLine("Null markables: " + CountNullMarkables(markables));

            NonDirectMarkables(markables);

            TextsMoreIndirectRelations( markables );
            TextsWithMoreChainsContainingAtLeatsOneIndirect( markables );

            Console.ReadKey();
            Console.ReadLine();
        }

        //elementos da cadeia que a qual ele pertence que vieram antes dele e que não sejam direto ordernados pela primeira palavra do span
        static void ElementosDaCadeiaAnterioresIndiretos(List<Pos> pos, List<Markable> markables)
        {
            List<Markable> indirects = markables.Where(x => x.
            IsAnaphoric == Indirect ).ToList();

            markables = markables.OrderBy(x => x.Text).ToList();
            List<string> texts = markables.Select(x => x.Text).Distinct().ToList();
            foreach (string text in texts)
            {
                Console.WriteLine("Text: " + text);
                List<Markable> textMarkables = markables.Where(x => x.Text == text).ToList();
                List<string> sets = textMarkables.Select(x => x.Member).Distinct().ToList();
                foreach (string set in sets)
                {
                    if (indirects.Any(x => x.Text == text && x.Member == set))
                    {
                        Console.WriteLine("Set " + set);
                        List<Markable> setMarkables = textMarkables.Where(x => x.Member == set).OrderBy(x => x.FirstWordIndex).ToList();
                        int lastIndirectIndex = setMarkables.IndexOf(setMarkables.Last(x => x.IsAnaphoric == Indirect));
                        for (int i=0; i<=lastIndirectIndex; i++)
                        {
                            Markable markable = setMarkables[i];
                            if (markable.IsAnaphoric != "direct")
                            {
                                string canon = "[" + string.Join("][", pos.Where(x => markable.SpanWordsId.Contains(x.Id) && x.Text == markable.Text).Select(x => x.Canon)) + "]";
                                Console.WriteLine(markable.Id + canon);
                            }
                        }
                    }
                }
            }
        }

        //quantos dos 331 indiretos são plural (que tem pelo menos um plural)
        static void IndiretosPlural(List<Pos> pos, List<Markable> markables)
        {
            List<Markable> indirects = markables.Where(x => x.IsAnaphoric == Indirect).ToList();
            int found = 0;
            foreach (Markable m in indirects)
            {
                if (m.SpanWordsId.Any(x => pos.Any(y => y.Text == m.Text && y.Id == x && y.Number == 'P')))
                    found++;
            }
            Console.WriteLine("Indirect plural: " + found);
        }

        // quantos markables estão com classificação nulo
        static int CountNullMarkables(List<Markable> markables)
        {
            return markables.Where(x => x.IsAnaphoric == null).Count();
        }

        // quantos markables tem de cada tipo não-direto
        static void NonDirectMarkables(List<Markable> markables)
        {
            IEnumerable<string> types = markables.Where(x => x.IsAnaphoric != null).Select(x => x.IsAnaphoric).Distinct();

            foreach (string t in types)
            {
                Console.WriteLine("[" + t + "]: " + markables.Where(x => x.IsAnaphoric == t).Count());
            }
        }

        static void TextsMoreIndirectRelations(List<Markable> markables)
        {
            Dictionary<string, int> textIndirectRelationsDictionary = new Dictionary<string, int>();

            foreach (Markable m in markables)
            {
                if (m.IsAnaphoric == Indirect )
                {
                    if ( textIndirectRelationsDictionary.ContainsKey( m.Text ) )
                        textIndirectRelationsDictionary[ m.Text ] += 1;
                    else
                        textIndirectRelationsDictionary.Add( m.Text, 1 );
                }
            }

            List<KeyValuePair<string, int>> textIndirectRelations = textIndirectRelationsDictionary
                .ToList()
                .OrderByDescending( x => x.Value )
                .ToList();

            Console.WriteLine( "3 texts with more indirect relations:" );
            foreach (KeyValuePair<string, int> k in textIndirectRelations.Take( 3 ))
                Console.WriteLine( "Text [" + k.Key + "] : " + k.Value );
        }

        static void TextsWithMoreChainsContainingAtLeatsOneIndirect(List<Markable> markables)
        {
            Dictionary<string, int> textChainsWithAtLeastOneIndirectDictionary = new Dictionary<string, int>();

            List<string> texts = markables.Select( x => x.Text ).Distinct().ToList();
            foreach (string t in texts)
            {
                textChainsWithAtLeastOneIndirectDictionary.Add( t, ChainsWithAtLeastOneIndirect( t, markables ) );
            }

            List<KeyValuePair<string, int>> textChainsWithAtLeastOneIndirect = textChainsWithAtLeastOneIndirectDictionary
                .ToList()
                .OrderByDescending( x => x.Value )
                .ToList();

            Console.WriteLine( "3 texts with more chains containing indirect relations:" );
            foreach ( KeyValuePair<string, int> k in textChainsWithAtLeastOneIndirect.Take( 3 ) )
                Console.WriteLine( "Text [" + k.Key + "] : " + k.Value );
        }

        static int ChainsWithAtLeastOneIndirect(string text, List<Markable> markables)
        {
            int chainsWithAtLeaseOneIndirect = 0;
            List<string> textChains = markables
                .Where( x => x.Text == text )
                .Select( x => x.Member )
                .Distinct()
                .ToList();

            foreach ( string c in textChains )
                if ( markables.Any( x => x.Member == c && x.IsAnaphoric == Indirect ) )
                    chainsWithAtLeaseOneIndirect++;

            return chainsWithAtLeaseOneIndirect;
        }
    }
}
