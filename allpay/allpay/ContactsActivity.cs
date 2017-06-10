
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
    [Activity(Label = "ContactsActivity")]
    public class ContactsActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // View
            SetContentView(Resource.Layout.Contacts);

        }
    }
}
