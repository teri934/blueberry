﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Essentials;
using Plugin.CurrentActivity;
using Dictionary.Implemented;

namespace Dictionary.Fragments
{
	class MainFragment : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_main, parent, false);

			Button recordings = view.FindViewById<Button>(Dictionary.Resource.Id.recordings_button);
			recordings.Click += Recordings_Click;

			Button settings = view.FindViewById<Button>(Dictionary.Resource.Id.settings_button);
			settings.Click += Settings_Click;

			Button quizes = view.FindViewById<Button>(Dictionary.Resource.Id.quizes_button);
			quizes.Click += Quizes_Click;

			return view;
		}

		/// <summary>
		/// function for recording button in the main menu, calls Recordings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Recordings_Click(object sender, EventArgs e)
		{
			Recordings();
		}

		/// <summary>
		/// function for settings button in the main menu, calls Settings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Settings_Click(object sender, EventArgs e)
		{
			Settings();
		}

		/// <summary>
		/// function for settings button in the main menu, calls Quizes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Quizes_Click(object sender, EventArgs e)
		{
			Quizes();
		}

		public void Recordings()
		{
			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new RecordingsFragment());
			ft.AddToBackStack(null);
			ft.Commit();
		}

		public void Settings()
		{
			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new SettingsFragment());
			ft.AddToBackStack(null);
			ft.Commit();
		}

		public void Quizes()
		{
			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new QuizesFragment());
			ft.AddToBackStack(null);
			ft.Commit();
		}
	}

	class RecordingsFragment : Android.Support.V4.App.Fragment
	{
		public static ListView myList;
		SearchView mySearchView;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_recordings, parent, false);
			MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;

			//finds search window and sets custom adapter to the ListView in which
			//the search results are displayed
			mySearchView = (SearchView)view.FindViewById(Dictionary.Resource.Id.searchView);
			myList = (ListView)view.FindViewById(Dictionary.Resource.Id.sounds_list);
			myList.Adapter = new RecordingAdapter(Context, activity.Dictionary);

			myList.TextFilterEnabled = false;
			SetUpSearchView();

			return view;
		}

		/// <summary>
		/// setting the design and functionality of the search window
		/// in the dictionary part of the app
		/// </summary>
		void SetUpSearchView()
		{
			mySearchView.SetIconifiedByDefault(false);
			mySearchView.SetOnQueryTextListener(new SearchViewListener());
			mySearchView.SubmitButtonEnabled = true;

			int id = Application.Context.Resources.GetIdentifier("@string/search_here", null, Application.Context.PackageName);
			string text = Application.Context.Resources.GetString(id);
			mySearchView.SetQueryHint(text);
		}
	}

	class SettingsFragment : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_settings, container, false);

			//switch button functionality
			Switch themeSwitch = (Switch)view.FindViewById(Dictionary.Resource.Id.modeSwitch);
			if (Preferences.Get("dark", false))
				themeSwitch.Checked = true;

			themeSwitch.SetOnCheckedChangeListener(new CompoundListener());

			return view;
		}
	}

	class QuizesFragment : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_quizes, container, false);

			Button scores = (Button)view.FindViewById(Dictionary.Resource.Id.scores_button);
			scores.Click += Scores_Click;

			Button start = (Button)view.FindViewById(Dictionary.Resource.Id.start_button);
			start.Click += Start_Click;

			return view;
		}

		/// <summary>
		/// function for start button in the quizes menu, starts the game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Start_Click(object sender, EventArgs e)
		{
			Game.round++;
			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new GameFragment(), "GameFragment");
			ft.AddToBackStack("game");
			ft.Commit();
		}

		/// <summary>
		/// function for scores button in the quizes menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Scores_Click(object sender, EventArgs e)
		{
			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Dictionary.Resource.Id.place_holder, new ScoresFragment());
			ft.AddToBackStack(null);
			ft.Commit();
		}
	}
}