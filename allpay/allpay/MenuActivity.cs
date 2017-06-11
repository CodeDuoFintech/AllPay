
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

namespace allpay
{
    [Activity(Label = "Menu")]
    public class MenuActivity : Activity
    {
        Button buttonMyQR;
        Button buttonPay;
        Button buttonContacts;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Menu);

			// MyQR button
            buttonMyQR = this.FindViewById<Button>(Resource.Id.buttonMyQR);
			buttonMyQR.Click += delegate
			{
                StartActivity(typeof(MyQRActivity));
			};

			// Pay button
			buttonPay = this.FindViewById<Button>(Resource.Id.buttonPay);
			buttonPay.Click += delegate
			{
                StartActivity(typeof(PayActivity));
			};

			// Contacts button
			buttonContacts = this.FindViewById<Button>(Resource.Id.buttonContacts);
            buttonContacts.Enabled = false; // ToDo: add application contact and enable money transfers to it
			buttonContacts.Click += delegate
			{
                StartActivity(typeof(ContactsActivity));
			};
        }
    }
}
