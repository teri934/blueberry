using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Util;
using Android.Text;
using Java.Lang;
using Language;

namespace Fragments
{
	class MainFragment : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_main, parent, false);
			Button recordings = view.FindViewById<Button>(Dictionary.Resource.Id.recordings_button);
			recordings.Click += Recordings_Click;
			return view;
		}

		void Recordings_Click(object sender, EventArgs e)
		{
			FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new RecordingsFragment());
			ft.AddToBackStack(null);
			ft.Commit();
		}
	}

	public static class ObjectTypeHelper
	{
		public static T Cast<T>(this Java.Lang.Object obj) where T : class
		{
			var propertyInfo = obj.GetType().GetProperty("Instance");
			return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
		}
	}

	class LineFilter : Filter
	{
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

		protected override FilterResults PerformFiltering(ICharSequence constraint)
		{
			FilterResults results = new FilterResults();
			List<Word> filteredList = new List<Word>();
			LineAdapter adapter = (LineAdapter)RecordingsFragment.myList.Adapter;

			if (adapter.originalData == null)
				adapter.originalData = new List<Word>(adapter.filteredData); //????

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
					string text = adapter.originalData[i].Original;
					if (text.ToLower().StartsWith(input))
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

	class Listener : Java.Lang.Object, SearchView.IOnQueryTextListener
	{
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

	class RecordingsFragment : Fragment
	{
		public static ListView myList;
		SearchView mySearchView;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_recordings, parent, false);

			mySearchView = (SearchView)view.FindViewById(Dictionary.Resource.Id.searchView);
			myList = (ListView)view.FindViewById(Dictionary.Resource.Id.sounds_list);
			myList.Adapter = new LineAdapter(Context, English.Dictionary);

			myList.TextFilterEnabled = false;
			SetUpSearchView();

			return view;
		}

		void SetUpSearchView()
		{
			mySearchView.SetIconifiedByDefault(false);
			mySearchView.SetOnQueryTextListener(new Listener());
			mySearchView.SubmitButtonEnabled = true;
			mySearchView.SetQueryHint("Search here");
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

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View v = convertView;
			ImageButton b;
			if (convertView == null)
			{
				v = inflater.Inflate(Dictionary.Resource.Layout.dict_line, null);
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
}