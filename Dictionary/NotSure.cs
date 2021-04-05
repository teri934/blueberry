using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using Android.Views;
using Android.Widget;
using Android.Accounts;
using Android.Text;
using Xamarin.Essentials;
using Java.Lang;
using Android.OS;

namespace Login
{
	class AccountAuthenticator : AbstractAccountAuthenticator
	{
		Context context;
		public AccountAuthenticator(Context context) : base(context)
		{
			this.context = context;
		}

		public override Bundle AddAccount(AccountAuthenticatorResponse response, string accountType, string authTokenType, string[] requiredFeatures, Bundle options)
		{
			Intent intent = new Intent(context, typeof(string));  //CustomServerAuthenticatorSigninActivity.Class
			intent.PutExtra(AccountGeneral.ACCOUNT_TYPE, accountType);
			intent.PutExtra(AccountGeneral.AUTHTOKEN_TYPE, authTokenType);
			intent.PutExtra(AccountGeneral.ADD_ACCOUNT, true);
			intent.PutExtra(AccountManager.KeyAccountAuthenticatorResponse, response);

			Bundle bundle = new Bundle();
			bundle.PutParcelable(AccountManager.KeyIntent, intent);
			return bundle;
		}

		public override Bundle ConfirmCredentials(AccountAuthenticatorResponse response, Account account, Bundle options)
		{
			return null;
		}

		public override Bundle EditProperties(AccountAuthenticatorResponse response, string accountType)
		{
			return null;
		}

		public override Bundle GetAuthToken(AccountAuthenticatorResponse response, Account account, string authTokenType, Bundle options)
		{
			// If the caller requested an authToken type we don't support, then
			// return an error
			if (authTokenType != AccountGeneral.AUTHTOKEN_TYPE_READ_ONLY && authTokenType != AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS)
			{
				Bundle result = new Bundle();
				result.PutString(AccountManager.KeyErrorMessage, "invalid authTokenType");
				return result;
			}

			// Extract the username and password from the Account Manager, and ask
			// the server for an appropriate AuthToken.
			AccountManager am = AccountManager.Get(context);
			string authToken = am.PeekAuthToken(account, authTokenType);

			// Lets give another try to authenticate the user
			if (TextUtils.IsEmpty(authToken))
			{
				string password = am.GetPassword(account);
				if (password != null)
				{
					try
					{
						//authToken = sServerAuthenticate.userSignIn(account.Name, password, authTokenType);
					}
					catch (System.Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}

			// If we get an authToken - we return it
			if (!TextUtils.IsEmpty(authToken))
			{
				Bundle result = new Bundle();
				result.PutString(AccountManager.KeyAccountName, account.Name);
				result.PutString(AccountManager.KeyAccountType, account.Type);
				result.PutString(AccountManager.KeyAuthtoken, authToken);
				return result;
			}

			// If we get here, then we couldn't access the user's password - so we
			// need to re-prompt them for their credentials. We do that by creating
			// an intent to display our AuthenticatorActivity.
			Intent intent = new Intent(context, typeof(string)); //AuthenticatorActivity.Class
			intent.PutExtra(AccountManager.KeyAccountAuthenticatorResponse, response);
			intent.PutExtra(AccountGeneral.ACCOUNT_TYPE, account.Type);
			intent.PutExtra(AccountGeneral.AUTHTOKEN_TYPE, authTokenType);
			intent.PutExtra(AccountGeneral.ACCOUNT_NAME, account.Name);
			Bundle bundle = new Bundle();
			bundle.PutParcelable(AccountManager.KeyIntent, intent);
			return bundle;
		}

		public override string GetAuthTokenLabel(string authTokenType)
		{
			if (AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS == authTokenType)
				return AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS_LABEL;
			else if (AccountGeneral.AUTHTOKEN_TYPE_READ_ONLY == authTokenType)
				return AccountGeneral.AUTHTOKEN_TYPE_READ_ONLY_LABEL;
			else
				return authTokenType + " (Label)";
		}

		public override Bundle HasFeatures(AccountAuthenticatorResponse response, Account account, string[] features)
		{
			Bundle result = new Bundle();
			result.PutBoolean(AccountManager.KeyBooleanResult, false);
			return result;
		}

		public override Bundle UpdateCredentials(AccountAuthenticatorResponse response, Account account, string authTokenType, Bundle options)
		{
			return null;
		}
	}

	public class AccountGeneral
	{
		public const string ACCOUNT_TYPE = "your_account_type_name";
		public const string ACCOUNT_NAME = "your_account_name";
		public const string AUTHTOKEN_TYPE = "your_account_name";
		public const string ADD_ACCOUNT = "Adding new account";


		public const string AUTHTOKEN_TYPE_READ_ONLY = "Read only";
		public const string AUTHTOKEN_TYPE_READ_ONLY_LABEL = "Read only access to an FitDodo account";

		public const string AUTHTOKEN_TYPE_FULL_ACCESS = "Full access";
		public const string AUTHTOKEN_TYPE_FULL_ACCESS_LABEL = "Full access to an FitDodo account";

		//public static ServerAuthenticate sServerAuthenticate = new ParseComServerAuthenticate();
	}
}