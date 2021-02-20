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

namespace Fragments
{
	class RecordingsFragment : Fragment
	{
        public override View OnCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
        {
            // Defines the xml file for the fragment
            return inflater.Inflate(Resource.Layout.main_recordings, parent, false);
        }
    }
}