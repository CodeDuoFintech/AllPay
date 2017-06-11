
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
using ZXing;


namespace allpay
{
    [Activity(Label = "MyQR")]

    public class MyQRActivity : Activity
    {
        ImageView imageMyQR;
        ContactClass myDetails = new ContactClass();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set view

            SetContentView(Resource.Layout.MyQR);

            // Generate QR code with details

            imageMyQR = FindViewById<ImageView>(Resource.Id.imageMyQR);

			var QRWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new ZXing.Common.EncodingOptions
				{
					Width = 600,
					Height = 600
				}
			};

            // QRType;ID;Name;Contact Type
            var MyQRCode = QRWriter.Write(myDetails.Type + ";" + 
                                          myDetails.ID + ";" + 
                                          myDetails.Name + ";" + 
                                          myDetails.LastName + ";" +
                                          myDetails.defaultCurrency + ";" +
                                          myDetails.alternateCurrency + ";" +
                                          myDetails.DOB + ";" +
                                          myDetails.defaultLanguage
                                         );

			imageMyQR.SetImageBitmap(MyQRCode);
        }
    }
}
