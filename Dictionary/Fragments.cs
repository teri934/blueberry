using System;
using Plugin.CurrentActivity;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Graphics;
using Xamarin.Essentials;
using Overriden;
using Dictionary;

namespace Fragments
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
			myList.Adapter = new LineAdapter(Context, activity.Dictionary);

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
			ft.Replace(Dictionary.Resource.Id.place_holder, new GameFragment());
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

	class ScoresFragment : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_scores, container, false);

			return view;
		}
	}

	class GameFragment : Android.Support.V4.App.Fragment
	{
		MediaPlayer player = new MediaPlayer();
		EditText input;
		TextView currentResult;
		Button button;
		MainActivity activity;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_game, container, false);
			activity = (MainActivity)CrossCurrentActivity.Current.Activity;

			Color color;
			if (Preferences.Get("dark", false))
				color = Color.LightSkyBlue;
			else
				color = Color.RoyalBlue;

			TextView question = (TextView)view.FindViewById(Dictionary.Resource.Id.question);
			question.Text = $"{MainActivity.GetLocalString("@string/question")}  {Game.round}/{Game.numberRounds}";
			question.SetTextColor(color);

			input = (EditText)view.FindViewById(Dictionary.Resource.Id.input_game);

			//generating random question from the dictionary
			Random rnd = new Random();
			Game.indexAudio = rnd.Next(0, activity.Dictionary.Count - 1);
			ImageButton recording = (ImageButton)view.FindViewById(Dictionary.Resource.Id.volume);
			recording.Click += Sound_Click;

			currentResult = (TextView)view.FindViewById(Dictionary.Resource.Id.current_result);

			//submit button is initiated
			button = (Button)view.FindViewById(Dictionary.Resource.Id.change_button);
			button.Text = MainActivity.GetLocalString("@string/submit_button");
			button.Click += Submit_Click;

			return view;
		}

		void Sound_Click(object sender, EventArgs e)
		{
			player.Release();
			player = MediaPlayer.Create(Application.Context, Application.Context.Resources.GetIdentifier(activity.Dictionary[Game.indexAudio].Filename, "raw", Application.Context.PackageName));
			player.Start();
		}

		/// <summary>
		/// function for submit button in the game
		/// changes the color of input text field according the correctnes of the answer and the theme
		/// prevents the user from entering text again
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Submit_Click(object sender, EventArgs e)
		{
			//color and text changes
			if (input.Text.ToLower() == activity.Dictionary[Game.indexAudio].Translation)
			{
				Color color;
				if (Preferences.Get("dark", false))
					color = Color.LawnGreen;
				else
					color = Color.DarkGreen;

				input.Background.Mutate().SetTint(color);

				currentResult.Text = MainActivity.GetLocalString("@string/toast_correct");
				currentResult.SetTextColor(color);
				Game.overallScore++;
			}
			else
			{
				input.Background.Mutate().SetTint(Color.Red);

				currentResult.Text = $"{MainActivity.GetLocalString("@string/toast_incorrect")}\n {MainActivity.GetLocalString("@string/correct_answer")} {activity.Dictionary[Game.indexAudio].Translation}";
				currentResult.SetTextColor(Color.Red);
			}

			//blocks user from further inputting in edittext
			input.FocusableInTouchMode = false;
			input.Focusable = false;

			//replaces submit button with next button
			button.Text = MainActivity.GetLocalString("@string/next_button");
			button.Click -= Submit_Click;
			button.Click += Next_Click;
		}

		/// <summary>
		/// continues to the next game question or overall results
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Next_Click(object sender, EventArgs e)
		{
			if (Game.round < Game.numberRounds)
			{
				Game.round++;
				Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
				ft.Replace(Dictionary.Resource.Id.place_holder, new GameFragment());
				//ft.AddToBackStack(null);
				ft.Commit();
			}
			else
			{
				Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
				ft.Replace(Dictionary.Resource.Id.place_holder, new ResultsFragment(), "ResultsFragment");
				//ft.AddToBackStack(null);
				ft.Commit();
			}
		}
	}

	struct Game
	{
		internal static int round = 0;
		internal const int numberRounds = 15;
		internal static int indexAudio = 0;
		internal static int overallScore = 0;
	}


	class ResultsFragment : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Dictionary.Resource.Layout.content_results, container, false);

			Game.round = 0;

			Color color;
			if (Preferences.Get("dark", false))
				color = Color.LightSkyBlue;
			else
				color = Color.RoyalBlue;

			TextView overall = (TextView)view.FindViewById(Dictionary.Resource.Id.overall_score);
			overall.Text = $"{MainActivity.GetLocalString("@string/overall_score")} {Game.overallScore}/{Game.numberRounds}";
			overall.SetTextColor(color);
			Game.overallScore = 0;

			return view;
		}

	}


	class GameDialog : Android.Support.V4.App.DialogFragment
	{
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);

			LayoutInflater inflater = RequireActivity().LayoutInflater;
			View view = inflater.Inflate(Dictionary.Resource.Layout.dialog_game, null);
			builder.SetView(view);

			Button yes = (Button)view.FindViewById(Dictionary.Resource.Id.yes_button);
			Button no = (Button)view.FindViewById(Dictionary.Resource.Id.no_button);

			yes.Click += Yes_Click;
			no.Click += No_Click;

			return builder.Create();
		}

		void Yes_Click(object sender, EventArgs e)
		{
			//Toast.MakeText(Application.Context, MainActivity.GetLocalString("@string/toast_language"), ToastLength.Short).Show();
			Dismiss();
			Game.round = 0;
			MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;

			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Remove(FragmentManager.FindFragmentById(Dictionary.Resource.Id.place_holder));
			ft.Commit();

			activity.CallInitializeMainFragment();
		}

		void No_Click(object sender, EventArgs e)
		{
			Dismiss();
		}
	}
}