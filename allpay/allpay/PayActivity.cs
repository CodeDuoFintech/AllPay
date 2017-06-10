
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ZXing.Mobile;
using Newtonsoft.Json;

namespace allpay
{
    [Activity(Label = "PayActivity")]
    public class PayActivity : Activity
    {
        Button buttonScan;

        TextView textViewBalance;
        TextView textViewConsole;

        MobileBarcodeScanner PayScanner;

        Double balance = 0.00;

        JsonValue json;

        APIClass apiCall = new APIClass();
        AccountClass account = new AccountClass();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileBarcodeScanner.Initialize(Application);

            // View
            SetContentView(Resource.Layout.Pay);

            // Scan payment QR

            textViewBalance = this.FindViewById<TextView>(Resource.Id.textViewBalance);
			textViewConsole = this.FindViewById<TextView>(Resource.Id.textViewConsole);
            buttonScan = this.FindViewById<Button>(Resource.Id.buttonScan);

            GetBalance();
            textViewBalance.Text += balance.ToString();

            if (balance <= 0.00)
            {
                buttonScan.Enabled = false;
                textViewConsole.Text = "Cannot make payment - not enough available funds";
            };

			buttonScan.Click += async delegate
            {

                PayScanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await PayScanner.Scan();

                ParseReceipt(result);
            };

        } // OnCreate

		void ParseReceipt(ZXing.Result result)
		{
			string paymentMsg = "You are about to pay EUR:";
            string[] paymentDetails = new string[] {};

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                paymentDetails = result.Text.Split(';');
                paymentMsg += paymentDetails[1].ToString(); // Amount
                paymentMsg +=  " to merchant with ID: " + paymentDetails[0].ToString(); // Merchant
                paymentMsg +=  " Receipt reference: " + paymentDetails[2].ToString(); // Receipt reference ID
			}
			else
				paymentMsg = "Scanning Canceled!";

            textViewConsole.Text = paymentMsg;
		} // ParseReceipt

        private async void GetBalance()
		{
            // ToDO: Dynamically build string
            string apiUrl = "Accounts/bda8eb884efcea209b2a6240";

            json = await apiCall.CallAPI(apiUrl);
            JsonValue result = json[0];
            //account = JsonConvert.DeserializeObject<AccountClass> (json);
            balance = result["balance"];

		} // Get Balance
	}
}
