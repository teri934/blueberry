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

namespace Fragments
{
	class RecordingsFragment : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			// Defines the xml file for the fragment
			return inflater.Inflate(Dictionary.Resource.Layout.content_recordings, parent, false);
		}

	}

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

			MediaPlayer player;
		}
	}
}