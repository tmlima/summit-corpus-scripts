using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummitRelations
{
    class Program
    {
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

            Console.ReadKey();
        }

        //elementos da cadeia que a qual ele pertence que vieram antes dele e que não sejam direto ordernados pela primeira palavra do span
        static void ElementosDaCadeiaAnterioresIndiretos(List<Pos> pos, List<Markable> markables)
        {
            List<Markable> indirects = markables.Where(x => x.IsAnaphoric == "indirect").ToList();
            Console.WriteLine("Indirects: " + indirects);

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
                        int lastIndirectIndex = setMarkables.IndexOf(setMarkables.Last(x => x.IsAnaphoric == "indirect"));
                        for (int i=0; i<=lastIndirectIndex; i++)
                        {
                            Markable markable = setMarkables[i];
                            if (markable.IsAnaphoric != "direct")
                            {
                                Console.WriteLine(markable.Id);
                            }
                        }
                    }
                }
            }
        }

        //quantos dos 331 indiretos são plural (que tem pelo menos um plural)
        static void IndiretosPlural(List<Pos> pos, List<Markable> markables)
        {
            List<Markable> indirects = markables.Where(x => x.IsAnaphoric == "indirect").ToList();
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
    }
}
