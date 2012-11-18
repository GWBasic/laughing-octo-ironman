using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace laughingoctoironman
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (4 != args.Length)
			{
				Console.WriteLine(
@"laughing-octo-ironman: Breaks a text file up into many xml documents. Creates a nifty coding challenge!

Usage: laughing-octo-ironman [text file] [template xml] [seed] [destination]

     [text file]:    A text file to break up, each line will result in a new
                     xml file with links to other lines

     [template xml]: An xml file to use as a template. Must have a tag with
                     its text contents @line@ for the line, @line_number@ for
                     the line number, and many @id@ for ids of other lines

     [seed]          A seed for the random number generator

     [destination]   Where to place the generated files

");

				return;
			}

			try
			{
				var textFilePath = args[0];
				var templateXmlPath = Path.GetFullPath(args[1]);
				var extension = Path.GetExtension(templateXmlPath);
				var seedText = args[2];
				var destinationPath = Path.GetFullPath(args[3]);

				int seed;
				try
				{
					seed = int.Parse(seedText);
				}
				catch
				{
					Console.WriteLine("Can not parse seed: {0}", seedText);
					return;
				}

				var random = new Random(seed);

				var textFile = File.ReadAllLines(textFilePath);

				var lines = new Line[textFile.Length];

				for (var ctr = 0; ctr < textFile.Length; ctr++)
				{
					var line = new Line(
						number: ctr,
						contents: textFile[ctr].Replace("\r", string.Empty).Replace("\n", string.Empty),
						random: random);

					lines[ctr] = line;
				}

				var templateXml = new XmlDocument();
				templateXml.Load(templateXmlPath);

				var lineNode = GetNode("@line@", templateXml.ChildNodes);
				var lineNumberNode = GetNode("@line_number@", templateXml.ChildNodes);
				var idNodes = GetNodes("@id@", templateXml.ChildNodes).ToArray();

				if (0 == idNodes.Length)
				{
					throw new Exception("There are no nodes with contents @id@");
				}

				var remainingIds = new HashSet<string>[idNodes.Length];
				for (var ctr = 0; ctr < idNodes.Length; ctr++)
				{
					remainingIds[ctr] = new HashSet<string>(lines.Select(line => line.Id));
				}

				foreach (var line in lines)
				{
					lineNode.InnerText = line.Contents;
					lineNumberNode.InnerText = line.Number.ToString();

					for (var ctr = 0; ctr < idNodes.Length; ctr++)
					{
						var idIndex = random.Next(0, remainingIds[ctr].Count);
						var id = remainingIds[ctr].ElementAt(idIndex);
						remainingIds[ctr].Remove(id);
						idNodes[ctr].InnerText = id;
					}

					templateXml.Save(Path.Combine(destinationPath, line.Id + extension));
				}
			}
			catch (Exception e)
			{
				var ex = e;

				do
				{
					Console.WriteLine(ex);
					ex = ex.InnerException;
				} while (null != ex);
			}
		}

		/// <summary>
		/// Gets the node with the contents
		/// </summary>
		private static XmlNode GetNode(string contents, XmlNodeList nodes)
		{
			try
			{
				return GetNodes(contents, nodes).First();
			}
			catch
			{
				throw new Exception("There is no node with contents: " + contents);
			}
		}

		/// <summary>
		/// Gets the nodes with the contents
		/// </summary>
		private static IEnumerable<XmlNode> GetNodes(string contents, XmlNodeList nodes)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.InnerText == contents)
				{
					yield return node;
				}

				foreach (var child in GetNodes(contents, node.ChildNodes))
				{
					yield return child;
				}
			}
		}
	}
}
