using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TicTacToe
{
	[Activity (Label = "TicTacToe", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			Button playerStartButton = FindViewById<Button> (Resource.Id.PlayerStartButton);
			Button computerStartButton = FindViewById<Button> (Resource.Id.PlayerStartButton);

			playerStartButton.Click += (sender, e) => 
			{
				StartActivity(typeof(PlayerGameActivity));
			};

			computerStartButton.Click += (sender, e) => 
			{
				StartActivity(typeof(PlayerGameActivity));
			};
		}
	}
}


