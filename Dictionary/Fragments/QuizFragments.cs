using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.Media;
using Android.Graphics;
using Xamarin.Essentials;
using Plugin.CurrentActivity;
using Dictionary;

namespace Fragments.Quizes
{
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
				ft.Replace(Dictionary.Resource.Id.place_holder, new GameFragment(), "GameFragment");
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
}