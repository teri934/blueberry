using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Util;
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

	class Listener : Java.Lang.Object, SearchView.IOnQueryTextListener
	{
		bool SearchView.IOnQueryTextListener.OnQueryTextChange(string newText)
		{
			//RecordingsFragment.arrayAdapter.GetFilter.
			return true;
		}

		bool SearchView.IOnQueryTextListener.OnQueryTextSubmit(string query)
		{
			return false;
		}
	}

	class RecordingsFragment : Fragment
	{
		ListView list;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_recordings, parent, false);

			list = (ListView)view.FindViewById(Dictionary.Resource.Id.sounds_list);
			list.Adapter = new LineAdapter(Context, English.Dictionary);

			return view;
		}
	}

	class LineAdapter : BaseAdapter
	{
		static MediaPlayer player = new MediaPlayer();
		List<Word> data;
		Android.Content.Context context;
		private static LayoutInflater inflater;
		public LineAdapter(Android.Content.Context context, List<Word> data)
		{
			this.data = data;
			this.context = context;
			inflater = (LayoutInflater)context.GetSystemService(Android.Content.Context.LayoutInflaterService);
		}

		public override int Count => data.Count;
		public override long GetItemId(int position) => 0;
		public override Java.Lang.Object GetItem(int position) => null;

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
			translation.Text = data[position].Translation;

			TextView original = (TextView)v.FindViewById(Dictionary.Resource.Id.original);
			int id = Application.Context.Resources.GetIdentifier(English.Dictionary[position].Original, null, Application.Context.PackageName);
			original.Text = Application.Context.Resources.GetString(id);

			b = (ImageButton)v.FindViewById(Dictionary.Resource.Id.volume);
			b.Tag = position;

			return v;
		}

		void Sound_Click(object sender, EventArgs e)
		{
			player.Release();
			player = MediaPlayer.Create(Application.Context, Application.Context.Resources.GetIdentifier(English.Dictionary[(int)((Button)sender).Tag].Filename, "raw", Application.Context.PackageName));
			player.Start();
		}
	}
}