using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Dictionary;

namespace Fragments
{
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

		/// <summary>
		/// removes the current fragment and changes dialog key in Preferences in order when calling OnBackPressed()
		/// the activity reacts correctly
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Yes_Click(object sender, EventArgs e)
		{
			Dismiss();
			MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;
			GameFragment fragment = (GameFragment)FragmentManager.FindFragmentByTag("GameFragment");
			Preferences.Set("dialog", "game");  //important

			Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Remove(fragment);
			ft.Commit();

			activity.OnBackPressed();
			Game.round = 0;
		}

		void No_Click(object sender, EventArgs e)
		{
			Dismiss();
		}
	}

	class LoginDialog : Android.Support.V4.App.DialogFragment
	{
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);

			LayoutInflater inflater = RequireActivity().LayoutInflater;
			View view = inflater.Inflate(Dictionary.Resource.Layout.dialog_login, null);
			builder.SetView(view);

			EditText username = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_username);
			EditText password = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_password);
			Button submit = (Button)view.FindViewById(Dictionary.Resource.Id.login_button);
			Button back = (Button)view.FindViewById(Dictionary.Resource.Id.back_button);

			submit.Click += Submit_Click;
			back.Click += Back_Click;

			return builder.Create();
		}

		private void Back_Click(object sender, EventArgs e)
		{
			Dismiss();
		}

		private void Submit_Click(object sender, EventArgs e)
		{
			Preferences.Set("user", true);
			Dismiss();
			MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;
			activity.RestartApp();
		}
	}

	class RegisterDialog : Android.Support.V4.App.DialogFragment
	{
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);

			LayoutInflater inflater = RequireActivity().LayoutInflater;
			View view = inflater.Inflate(Dictionary.Resource.Layout.dialog_signup, null);
			builder.SetView(view);

			EditText username = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_username);
			EditText name = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_name);
			EditText surname = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_surname);
			EditText password = (EditText)view.FindViewById(Dictionary.Resource.Id.prompt_password);
			Button submit = (Button)view.FindViewById(Dictionary.Resource.Id.login_button);
			Button back = (Button)view.FindViewById(Dictionary.Resource.Id.back_button);

			submit.Click += Submit_Click;
			back.Click += Back_Click;

			return builder.Create();
		}

		private void Back_Click(object sender, EventArgs e)
		{
			Dismiss();
		}

		private void Submit_Click(object sender, EventArgs e)
		{
			Preferences.Set("user", true);
			Dismiss();
			MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;
			activity.RestartApp();
		}
	}
}