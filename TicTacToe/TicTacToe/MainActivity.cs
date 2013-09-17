using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace TicTacToe
{
	[Activity (Label = "Tic Tac Toe", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			Button playerStartButton = FindViewById<Button> (Resource.Id.PlayerStartButton);
			Button computerStartButton = FindViewById<Button> (Resource.Id.ComputerStartButon);

			playerStartButton.Click += (sender, e) => 
			{
				var gameActivity = new Intent (this, typeof(PlayerGameActivity));
				gameActivity.PutExtra("PlayerStart", true);
				StartActivity(gameActivity);
			};

			computerStartButton.Click += (sender, e) => 
			{
				var gameActivity = new Intent (this, typeof(PlayerGameActivity));
				gameActivity.PutExtra("PlayerStart", false);
				StartActivity(gameActivity);
			};
		}
	}
}


