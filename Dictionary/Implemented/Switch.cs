using Android.Widget;
using Xamarin.Essentials;
using Plugin.CurrentActivity;
using Dictionary;

namespace Dictionary.Implemented
{
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
				Preferences.Set(MainActivity.dark, true);
			else
				Preferences.Set(MainActivity.dark, false);

			((MainActivity)CrossCurrentActivity.Current.Activity).RestartApp();
		}
	}
}