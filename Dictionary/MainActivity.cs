using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Xamarin.Essentials;
using Language;
using Overriden;
using Android.Widget;

namespace Dictionary
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public static MainActivity instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            instance = this;

			//Preferences used here to remember if the app was
            //in a day or night mode before closing
            if (Preferences.Get("dark", false))
                SetTheme(Resource.Style.DarkTheme);
			else
				SetTheme(Resource.Style.AppTheme);

			//initialisation of needed components
			base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.ItemIconTintList = null;
            navigationView.SetNavigationItemSelectedListener(this);


            //main menu is dynamically created
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.place_holder, new Fragments.MainFragment());
            ft.Commit();

            //at the start of the app the method CreateDictionary is called
            //to read the needed data for further usage
            ILanguage en = new English();
            en.CreateDictionary();
        }

        /// <summary>
        /// behaviour (of main drawer) when back tab is pressed
        /// </summary>
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.english)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// restarting the app after changing the view mode (light or dark)
        /// </summary>
        public void RestartApp()
        {
            Intent i = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(i);
            Finish();
        }
    }
}

