using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Util;

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

	class RecordingsFragment : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_recordings, parent, false);
			//LinearLayout list = view.FindViewById<LinearLayout>(Dictionary.Resource.Id.sounds_list);
			//list.AddView(new Button(Application.Context));
			Log.Debug("f", "recordings");

			FragmentTransaction ft = FragmentManager.BeginTransaction();
			for (int i = 0; i < 30; i++)
			{
				ft.Add(Dictionary.Resource.Id.sounds_list, new SoundsFragment());
			}
			ft.Commit();

			return view;
		}
	}

	class SoundsFragment : Fragment
	{
		static MediaPlayer player = new MediaPlayer();
		string sound;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.sound_button, parent, false);
			Button b = (Button)((ViewGroup)view).GetChildAt(0);
			b.Text = sound = "banana";
			b.Click += Sound_Click;
			Log.Debug("f", "sound");
			return view;
		}

		void Sound_Click(object sender, EventArgs e)
		{
			player.Release();
			player = MediaPlayer.Create(Application.Context, Application.Context.Resources.GetIdentifier(sound, "raw", Application.Context.PackageName));
			player.Start();
			Log.Debug("f", "buttonpress");
		}
	}
}