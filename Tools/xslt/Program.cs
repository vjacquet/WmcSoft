using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

class Program
{
    static void Main(string[] args)
    {
        var doc = new XPathDocument(args[0]);
        //// Locate the node fragment.
        //var selection = doc.CreateNavigator().SelectSingleNode("descendant::book[@ISBN = '0-201-63361-2']");
        //var reader = selection.ReadSubtree();
        //reader.MoveToContent();

        var xslt = new XslCompiledTransform();
        if (args.Length > 1) {
            xslt.Load(args[1]);
        } else {
            var nav = doc.CreateNavigator();
            var pi = nav.SelectSingleNode(@"/descendant-or-self::processing-instruction(""xml-stylesheet"")");
            if (pi != null) {
                var regex = new Regex("href=\"([^\"]*)\"");
                var match = regex.Match(pi.Value);
                if (match.Success) {
                    string text = match.Groups[1].Value.Replace('/', Path.DirectorySeparatorChar);
                    xslt.Load(Path.Combine(args[0], "..\\" + text));
                }
            }
        }
        xslt.Transform(doc, XmlWriter.Create(Console.Out, xslt.OutputSettings));
    }
}
