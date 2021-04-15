using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Media;
using Android.Accounts;
using Android.Text;
using Xamarin.Essentials;
using Java.Lang;
using Language;
using Dictionary;
using Android.OS;
using Android.Runtime;
using Fragments.MainMenu;

namespace Overwritten
{
	class SearchViewListener : Java.Lang.Object, SearchView.IOnQueryTextListener
	{
		/// <summary>
		/// sets listener to the search window in the dictionary part of the app
		/// it detects when something changes in the input text field and invokes the filter
		/// </summary>
		/// <param name="newText">input from user</param>
		/// <returns></returns>
		bool SearchView.IOnQueryTextListener.OnQueryTextChange(string newText)
		{
			LineAdapter adapter = (LineAdapter)RecordingsFragment.myList.Adapter;
			LineFilter filter = (LineFilter)adapter.Filter;
			filter.InvokeFilter(newText);

			return true;
		}

		bool SearchView.IOnQueryTextListener.OnQueryTextSubmit(string query)
		{
			return false;
		}
	}

	class LineAdapter : BaseAdapter, IFilterable
	{
		static MediaPlayer player = new MediaPlayer();
		public List<Word> originalData;
		public List<Word> filteredData;
		Android.Content.Context context;
		private static LayoutInflater inflater;
		public LineAdapter(Android.Content.Context context, List<Word> data)
		{
			originalData = data;
			filteredData = data;
			this.context = context;
			inflater = (LayoutInflater)context.GetSystemService(Android.Content.Context.LayoutInflaterService);
		}

		public override int Count => filteredData.Count;
		public override long GetItemId(int position) => position;
		public override Java.Lang.Object GetItem(int position) => filteredData[position];

		public Filter Filter => new LineFilter();

		/// <summary>
		/// sets the right text and sound data to the TextViews and ImageButton in dict_line
		/// this is done for every line when it is supposed to be displayed
		/// </summary>
		/// <param name="position">index in the filteredData</param>
		/// <param name="convertView">current view</param>
		/// <param name="parent"></param>
		/// <returns>new View</returns>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View v = convertView;
			ImageButton b;
			if (convertView == null)
			{
				v = inflater.Inflate(Dictionary.Resource.Layout.dictionary_line, null);
				b = (ImageButton)v.FindViewById(Dictionary.Resource.Id.volume);
				b.Click += Sound_Click;
			}

			TextView translation = (TextView)v.FindViewById(Dictionary.Resource.Id.translation);
			translation.Text = filteredData[position].Translation;

			TextView original = (TextView)v.FindViewById(Dictionary.Resource.Id.original);
			original.Text = filteredData[position].Original;

			b = (ImageButton)v.FindViewById(Dictionary.Resource.Id.volume);
			b.Tag = position;

			return v;
		}

		void Sound_Click(object sender, EventArgs e)
		{
			player.Release();
			player = MediaPlayer.Create(Application.Context, Application.Context.Resources.GetIdentifier(filteredData[int.Parse(((ImageButton)sender).Tag.ToString())].Filename, "raw", Application.Context.PackageName));
			player.Start();
		}
	}

	class LineFilter : Filter
	{
		/// <summary>
		/// sets the right (filtered data) to list of the custom adapter
		/// which will be displayed
		/// </summary>
		/// <param name="constraint">user text input</param>
		/// <param name="results">filter results form the method PerformFiltering</param>
		protected override void PublishResults(ICharSequence constraint, FilterResults results)
		{
			LineAdapter adapter = (LineAdapter)RecordingsFragment.myList.Adapter;
			List<Word> filteredList = new List<Word>();
			for (int i = 0; i < results.Count; i++)
			{
				filteredList.Add((Word)((Java.Lang.Object[])results.Values)[i]);
			}
			adapter.filteredData = filteredList;
			adapter.NotifyDataSetChanged();
		}

		/// <summary>
		/// filters the original list of data according to the user input
		/// (filtering is based on the native language)
		/// </summary>
		/// <param name="constraint">user text input<</param>
		/// <returns>results of the filtering</returns>
		protected override FilterResults PerformFiltering(ICharSequence constraint)
		{
			FilterResults results = new FilterResults();
			List<Word> filteredList = new List<Word>();
			LineAdapter adapter = (LineAdapter)RecordingsFragment.myList.Adapter;

			if (adapter.originalData == null)
				adapter.originalData = new List<Word>(adapter.filteredData);

			if (constraint == null || constraint.Length() == 0)
			{
				//originalData are set to results
				results.Count = adapter.originalData.Count;
				results.Values = adapter.originalData.ToArray();
			}
			else
			{
				string input = constraint.ToString().ToLower();

				for (int i = 0; i < adapter.originalData.Count; i++)
				{
					string original = adapter.originalData[i].Original;
					string translation = adapter.originalData[i].Translation;
					if (original.ToLower().StartsWith(input) || translation.ToLower().StartsWith(input))
					{
						filteredList.Add(adapter.originalData[i]);
					}
				}

				//filtered data (filteredList) is set to results
				results.Count = filteredList.Count;
				results.Values = filteredList.ToArray();
			}

			return results;
		}
	}

	class CompoundListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
	{
		/// <summary>
		/// sets in the dictionary of preferences true or false according to the
		/// state of switch button for dark/light mode
		/// </summary>
		/// <param name="buttonView"></param>
		/// <param name="isChecked"></param>
		public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			if (isChecked)
				Preferences.Set("dark", true);
			else
				Preferences.Set("dark", false);

			((MainActivity)CrossCurrentActivity.Current.Activity).RestartApp();
		}

	}

	class TextViewListener : Java.Lang.Object, TextView.IOnEditorActionListener
	{
		public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
		{
			//EditorInfo editor = new EditorInfo();

			//if ((int)actionId == Dictionary.Resource.Id.something || actionId == 0)  //not sure EditorInfo.IME_NULL
			//{
			//	AttemptLogin();
			//	return true;
			//}

			return false;
		}

		void AttemptLogin()
		{

		}
	}

}