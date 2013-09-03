using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TicTacToe
{
	[Activity (Label = "PlayerGameActivity")]			
	public class PlayerGameActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.PlayerGameLayout);
	
			var button1 = FindViewById<Button> (Resource.Id.button1);
			var button2 = FindViewById<Button> (Resource.Id.button2);
			var button3 = FindViewById<Button> (Resource.Id.button3);
			var button4 = FindViewById<Button> (Resource.Id.button4);
			var button5 = FindViewById<Button> (Resource.Id.button5);
			var button6 = FindViewById<Button> (Resource.Id.button6);
			var button7 = FindViewById<Button> (Resource.Id.button7);
			var button8 = FindViewById<Button> (Resource.Id.button8);
			var button9 = FindViewById<Button> (Resource.Id.button9);

			var restartButton = FindViewById<Button> (Resource.Id.RestartButton);

			restartButton.Click += (sender, e) => 
			{
				StartActivity(typeof(MainActivity));
			};

		}
	}
}

