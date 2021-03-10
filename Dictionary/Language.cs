using System.Collections.Generic;
using System.IO;
using Android.Content.Res;
using Android.Util;
using Org.Json;
using System.Linq;
using Android.App;

namespace Language
{
	class English : ILanguage
	{
		public Languages language => Languages.english;
		const string lang = "english";
		static List<Word> sortedDictionary;
		public static List<Word> Dictionary => sortedDictionary;

		public void CreateDictionary()
		{
			AssetManager assets = Application.Context.Assets;
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
				Word word = Word.CreateNewWord((JSONObject)ar.Get(i), lang);
				dictionary.Add(word);
			}

			sortedDictionary = Word.SortList(dictionary);
		}
	}

	enum Languages
	{
		english
	}

	interface ILanguage
	{
		/// <summary>
		/// reads content from json file, stores it into JSONArray and
		/// then, by calling functions from the class Word, creates sorted dictionary of Words
		/// </summary>
		public void CreateDictionary();
		public Languages language { get; }
	}

	class Word : Java.Lang.Object
	{
		 public string Filename { get; private set; }
		 public string Translation { get; private set; }
		 public string Original { get; private set; }

		/// <summary>
		/// creates new object of Word (one line in the app dictionary) from JSONObject
		/// </summary>
		/// <param name="ob">current item from JSONArray ar</param>
		/// <param name="language">language of translation (until further support default english)</param>
		/// <returns>object of the class Word</returns>
		public static Word CreateNewWord(JSONObject ob, string language)
		{
			int id = Application.Context.Resources.GetIdentifier(ob.Get("original").ToString(), null, Application.Context.PackageName);
			return new Word { Filename = ob.Get("filename").ToString(), Translation = ob.Get(language).ToString(), Original = Application.Context.Resources.GetString(id)};
		}

		/// <summary>
		/// sorts list according to Original in Word (native language of the environment)
		/// </summary>
		/// <param name="list">list of Words generated from json file</param>
		/// <returns>sorted list</returns>
		public static List<Word> SortList(List<Word> list)
		{
			var sortedQuery = list.OrderBy(x => x.Original);
			return sortedQuery.ToList();
		}
	}
}