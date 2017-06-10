using Android.App;
using Android.Widget;
using Android.OS;

namespace allpay
{
    [Activity(Label = "allpay", MainLauncher = true, Icon = "@mipmap/icon")]

    public class MainActivity : Activity
    {
        Button buttonLogin;

        EditText editUsername;
        EditText editPassword;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Login
            buttonLogin = this.FindViewById<Button>(Resource.Id.buttonLogin);
            buttonLogin.Click += delegate
            {
                //ToDO: Implement proper login mechanism

                StartActivity(typeof(MenuActivity));
            };

        }
    }
}

