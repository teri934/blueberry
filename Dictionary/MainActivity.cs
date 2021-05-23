using System.Threading.Tasks;
using System.Collections.Generic;
using Android.App;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Graphics;
using Android.Text.Style;
using Xamarin.Essentials;
using Android.Widget;
using Language;
using Dictionary.Fragments;
using Dictionary.Database;

namespace Dictionary
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        Languages language;
        const string white = "#FFFFFF";
        const string black = "#000000";
        public const string nothing = "nothing";
        public const string dialog = "dialog";
        public const string game = "game";
        public const string dark = "dark";
        ResultsDatabase database;
        public List<Word> Dictionary { get; private set;}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);

            bool dark;
            Preferences.Set(dialog, nothing);

			//Preferences used here to remember if the app was
            //in a day or night mode before closing
            if (dark = Preferences.Get("dark", false))
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
            //UpdateUserInfo(navigationView.GetHeaderView(0));
            navigationView.ItemIconTintList = null;
            navigationView.SetNavigationItemSelectedListener(this);

            if (dark)
                CustomizeNavigationMenu(navigationView, Color.LawnGreen, white);
            else
                CustomizeNavigationMenu(navigationView, Color.DarkGreen, black);

            //main menu is dynamically created
            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.place_holder, new MainFragment());
            ft.Commit();

            //at the start of the app the method CreateDictionary is called
            //to read the needed data for further usage
            language = English.language;
            English.CallCreateDictionary(new English());
            Dictionary = English.Dictionary;

            database = Task.Run(() => ResultsDatabase.CreateDatabase()).GetAwaiter().GetResult();
        }


        /// <summary>
        /// sets different colors for menu items according to the current theme
        /// </summary>
        /// <param name="navigationView"></param>
        /// <param name="color"></param>
        void CustomizeNavigationMenu(NavigationView navigationView, Color colorText, string colorItem)
		{
            for (int i = 0; i < navigationView.Menu.Size(); i++)
            {
                IMenuItem item = navigationView.Menu.GetItem(i);
                Android.Text.SpannableString spanString = new Android.Text.SpannableString(navigationView.Menu.GetItem(i).ToString());
                spanString.SetSpan(new ForegroundColorSpan(colorText), 0, spanString.Length(), 0);
                item.SetTitle(spanString);

                LoopSubmenu(item, colorItem);
            }
        }

        /// <summary>
        /// sets different colors for menu icons (black or white) according to the current theme
        /// </summary>
        /// <param name="item"></param>
        /// <param name="colorItem"></param>
        void LoopSubmenu(IMenuItem item, string colorItem)
		{
            if (item.ToString() == "Menu")
            {
                IMenuItem icon;
                for (int i = 0; i < item.SubMenu.Size(); i++)
                {
                    icon = item.SubMenu.GetItem(i);
                    icon.Icon.SetTint(Color.ParseColor(colorItem));
                }
            }
        }

        /// <summary>
        /// behaviour (of main drawer) when back tab is pressed
        /// </summary>
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ResultsFragment fragment = (ResultsFragment)SupportFragmentManager.FindFragmentByTag("ResultsFragment");

            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else if (fragment != null)  //the user ended the quiz
			{
                Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
                ft.Remove(fragment);
                ft.Commit();
                base.OnBackPressed();
            }
            else if(Preferences.Get(dialog, nothing) == game)  //dialog window is already open
			{
                Preferences.Set(dialog, nothing);
                base.OnBackPressed();
			}
            else if(Game.round >= 1 && Game.round <= Game.numberRounds)  //quiz is in the process
			{
                GameDialog dialog = new GameDialog();
                dialog.Show(SupportFragmentManager, null);
			}
            else
            {
                base.OnBackPressed();
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.english)
            {
                if(language == Languages.english)
				{
                    Toast.MakeText(Application.Context, GetLocalString("@string/toast_language"), ToastLength.Short).Show();
                    return true;
                }
            }
            else
			{
                RemoveFromBackStack();

                MainFragment f = new MainFragment();
                InitializeMainFragment(f);

                if (id == Resource.Id.recordings_item)
                    f.Recordings();
                else if (id == Resource.Id.settings_item)
                    f.Settings();
                else if (id == Resource.Id.quizes_item)
                    f.Quizes();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// after removing all fragments from stack, the main fragment (the main screen with menu buttons)
        /// needs to initialized
        /// </summary>
        /// <param name="f"></param>
        void InitializeMainFragment(MainFragment f)
		{
            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.place_holder, f);
            ft.Commit();
        }

        /// <summary>
        /// removes from back stack all fragments, so when changing to another window in nav drawer
        /// the button back returns the user always to the main menu screen
        /// </summary>
        void RemoveFromBackStack()
		{
            Android.Support.V4.App.FragmentManager manager = SupportFragmentManager;

            for (int i = 0; i < manager.BackStackEntryCount; i++)
            {
                manager.PopBackStack();
            }
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

        public static string GetLocalString(string strId)
		{
            int id = Application.Context.Resources.GetIdentifier(strId, null, Application.Context.PackageName);
            return Application.Context.Resources.GetString(id);
        }

        public void CallInitializeMainFragment()
		{
            InitializeMainFragment(new MainFragment());
		}
    }
}

