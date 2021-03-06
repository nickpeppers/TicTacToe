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
using Android.Content.PM;
using Android.Views.Animations;
using System.Threading;
using TicTacToe;

namespace WKUACM.Activities
{
    [Activity(MainLauncher = true, Icon = "@drawable/Icon", ScreenOrientation=ScreenOrientation.Portrait, ConfigurationChanges=ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        private Animation _fadeIn,
             _fadeOut;
        private RelativeLayout _splashLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SplashScreenLayout);

            _splashLayout = FindViewById<RelativeLayout>(Resource.Id.SplashScreenLayout);
            _splashLayout.Visibility = ViewStates.Invisible;
            _fadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.splash_fade_in);
            _fadeOut = AnimationUtils.LoadAnimation(this, Resource.Animation.splash_fade_out);

            _fadeIn.AnimationEnd += (sender, e) =>
            {
                _splashLayout.StartAnimation(_fadeOut);
            };

            _fadeOut.AnimationEnd += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                _splashLayout.Visibility = ViewStates.Gone;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Finish();
                });
            };
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            ThreadPool.QueueUserWorkItem(_ =>
            {
                RunOnUiThread(() =>
                {
                    _splashLayout.Visibility = ViewStates.Visible;
                    _splashLayout.StartAnimation(_fadeIn);
                });
            });
        }
    }
}