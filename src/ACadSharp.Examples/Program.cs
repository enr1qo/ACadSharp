using ACadSharp.IO;
using ACadSharp.Tables;
using ACadSharp.Tables.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using ACadSharp.Entities;
using ACadSharp.Objects.Evaluations;
using CSMath;

namespace ACadSharp.Examples
{
	class Program
	{
		private const string _file = "C:\\Users\\panchukvv\\Documents\\test_dyn_insert.dwg";

		const string _dynBlockName = "FSA_Heater_W";

		//const string _dynBlockName = "CircleOrSquare";
		private static List<String> _visibilitis = new List<String>()
		{
			"0",
			"FS",
			"PS"
			//"circle",
			//"square"
		};

		private static List<XYZ> _insertPoints = new List<XYZ>()
		{
			new XYZ(0, 0, 0),
			new XYZ(150, 0, 0),
			new XYZ(300, 0, 0)
		};

		private const string _dinName = "SensorTypeIn";

		static void Main( string[] args )
		{
			CadDocument doc;
			DwgPreview preview;
			using (DwgReader reader = new DwgReader(_file))
			{
				doc = reader.Read();
				preview = reader.ReadPreview();

				var blockRecord = doc.BlockRecords[_dynBlockName];
				BlockVisibilityParameter dynamicBLock = null;
				if (blockRecord.XDictionary != null && blockRecord.XDictionary.EntryNames.Contains("ACAD_ENHANCEDBLOCK"))
				{
					var enhancedBlock = blockRecord.XDictionary["ACAD_ENHANCEDBLOCK"] as EvaluationGraph;
					if (enhancedBlock != null && enhancedBlock is EvaluationGraph)
					{
						foreach (EvaluationGraph.Node node in enhancedBlock.Nodes)
						{
							if (node.Expression is BlockVisibilityParameter)
							{
								dynamicBLock = (BlockVisibilityParameter)node.Expression;
								break;
							}
						}
					}
				}
			}

			//exploreDocument(doc);

			Console.ReadKey();
		}

		/// <summary>
		/// Logs in the console the document information
		/// </summary>
		/// <param name="doc"></param>
		static void exploreDocument(CadDocument doc)
		{
			Console.WriteLine();
			Console.WriteLine("SUMMARY INFO:");
			Console.WriteLine($"\tTitle: {doc.SummaryInfo.Title}");
			Console.WriteLine($"\tSubject: {doc.SummaryInfo.Subject}");
			Console.WriteLine($"\tAuthor: {doc.SummaryInfo.Author}");
			Console.WriteLine($"\tKeywords: {doc.SummaryInfo.Keywords}");
			Console.WriteLine($"\tComments: {doc.SummaryInfo.Comments}");
			Console.WriteLine($"\tLastSavedBy: {doc.SummaryInfo.LastSavedBy}");
			Console.WriteLine($"\tRevisionNumber: {doc.SummaryInfo.RevisionNumber}");
			Console.WriteLine($"\tHyperlinkBase: {doc.SummaryInfo.HyperlinkBase}");
			Console.WriteLine($"\tCreatedDate: {doc.SummaryInfo.CreatedDate}");
			Console.WriteLine($"\tModifiedDate: {doc.SummaryInfo.ModifiedDate}");

			exploreTable(doc.AppIds);
			exploreTable(doc.BlockRecords);
			exploreTable(doc.DimensionStyles);
			exploreTable(doc.Layers);
			exploreTable(doc.LineTypes);
			exploreTable(doc.TextStyles);
			exploreTable(doc.UCSs);
			exploreTable(doc.Views);
			exploreTable(doc.VPorts);
		}

		static void exploreTable<T>(Table<T> table)
			where T : TableEntry
		{
			Console.WriteLine($"{table.ObjectName}");
			foreach (var item in table)
			{
				Console.WriteLine($"\tName: {item.Name}");

				if (item.Name == BlockRecord.ModelSpaceName && item is BlockRecord model)
				{
					Console.WriteLine($"\t\tEntities in the model:");
					foreach (var e in model.Entities.GroupBy(i => i.GetType().FullName))
					{
						Console.WriteLine($"\t\t{e.Key}: {e.Count()}");
					}
				}
			}
		}
	}
}