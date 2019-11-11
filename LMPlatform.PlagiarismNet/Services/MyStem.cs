using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LMPlatform.PlagiarismNet.Utils;
using LMPlatform.PlagiarismNet.Services.Interfaces;

namespace LMPlatform.PlagiarismNet.Services
{
	public class MyStem : IMyStem
	{
		public static readonly string MYSTEM_DIR = @"D:\LMSSystem" + Path.DirectorySeparatorChar + "mystem";

		public static readonly string MYSTEM_IN_DIR = @"D:\LMSSystem" + Path.DirectorySeparatorChar;

		public static readonly string MYSTEM_OUT_DIR = @"D:\LMSSystem" + Path.DirectorySeparatorChar;

		public const string MUTEX = "mutex";


		public MyStem()
		{
		}

		public List<string> Parse(string fileName)
		{

			List<string> terms = new List<string>();
			
			try
			{
				terms.AddRange(PopulateTermsList(GetText(Path.GetFullPath(fileName))));
			}
			catch (Exception var4)
			{

				Console.WriteLine(var4.StackTrace);

			}

			return terms;

		}

		public static List<string> PopulateTermsList(List<string> termsList)
		{
			var terms = new List<string>();
			termsList.ForEach(x =>
			{
				var words = x.Split(' ');
				foreach (var word in words)
				{
					terms.Add(word);
				}
			});
			terms.RemoveAll(x => x == "");

			return terms;
		}


		private static List<string> GetText(string filepath)
		{

			WordprocessingDocument wordDoc = WordprocessingDocument.Open(filepath, true);

			var termsList = new List<string>();

			var doc = wordDoc.MainDocumentPart.Document;
			foreach (var paragraph in doc.Body.ChildElements)
			{
				// Iterate through all Run elements in the Paragraph element.
				foreach (var run in paragraph.ChildElements)
				{
					string text = run.InnerText;
					if (!string.IsNullOrEmpty(text))
					{
						termsList.Add(text);
					}
				}
			}
			wordDoc.Close();
			return termsList;
		}
	}
}
