using System;

namespace laughingoctoironman
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (2 != args.Length)
			{
				Console.WriteLine(
@"laughing-octo-ironman: Breaks a text file up into many xml documents. Creates a nifty coding challenge!

Usage: laughing-octo-ironman [text file] [template xml]
     [text file]:    A text file to break up, each line will result in a new
                     xml file with links to other lines
     [template xml]: An xml file to use as a template. Must have a tag with
                     its text contents @line@ for the line, and many
                     @hashlink@ for hashes of other lines

");

				return;
			}

			var textFile = args[0];
			var templateXml = args[1];
		}
	}
}
