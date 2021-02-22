using System;
using System.Collections.Generic;
using System.IO;
using Android.Content.Res;
using Android.Util;
using Org.Json;
using System.Linq;

namespace Language
{
	class English : ILanguage
	{
		const string language = "english";
		static List<Word> sortedDictionary;
		public static List<Word> Dictionary => sortedDictionary;
		public void CreateDictionary()
		{
			AssetManager assets = Android.App.Application.Context.Assets;
			string content = "";
			try
			{
				using (StreamReader sr = new StreamReader(assets.Open("dictionary.json")))
				{
					content = sr.ReadToEnd();
				}
			}
			catch (IOException) { Log.Debug("Language", "There was an exception when reading a file"); }

			JSONArray ar = new JSONArray(content);
			List<Word> dictionary = new List<Word>();

			for (int i = 0; i < ar.Length(); i++)
			{
				Word word = Word.CreateNewWord((JSONObject)ar.Get(i), language);
				dictionary.Add(word);
			}

			sortedDictionary = Word.SortList(dictionary);
		}
	}

	interface ILanguage
	{
		public void CreateDictionary();
	}

	class Word
	{
		 public string Filename { get; private set; }
		 public string Translation { get; private set; }
		 public string Original { get; private set; }

		public static Word CreateNewWord(JSONObject ob, string lang)
		{
			return new Word { Filename = ob.Get("filename").ToString(), Translation = ob.Get(lang).ToString(), Original = ob.Get("original").ToString() };
		}

		public static List<Word> SortList(List<Word> list)
		{
			var sortedQuery = list.OrderBy(x => x.Original);
			return sortedQuery.ToList();
		}
	}
}