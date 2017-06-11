
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

namespace allpay
{
    [Activity(Label = "PayActivity")]
    public class PayActivity : Activity
    {
        Button buttonScan;
        Button buttonNewPayment;

        TextView textViewBalance;
        TextView textViewConsole;

        string paymentMsg = "You are about to pay ";
        string merchantID;
        string referenceID;
        string currency;
        Double i004_trxn_amount = 0.00;

        MobileBarcodeScanner PayScanner;

        Double balance = 0.00;

        JsonValue json;

        APIClass apiCall = new APIClass();
        readonly ContactClass myDetails = new ContactClass();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileBarcodeScanner.Initialize(Application);

            // View
            SetContentView(Resource.Layout.Pay);

            // Scan payment QR

            textViewBalance = this.FindViewById<TextView>(Resource.Id.textViewBalance);
			textViewConsole = this.FindViewById<TextView>(Resource.Id.textViewConsole);
            buttonNewPayment = this.FindViewById<Button>(Resource.Id.buttonNewPayment);
            buttonScan = this.FindViewById<Button>(Resource.Id.buttonScan);
            buttonScan.Enabled = false; // Enable only if account has balance

            GetBalance();

            buttonScan.Click += async delegate
            {

                PayScanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await PayScanner.Scan();

                ParseReceipt(result);
            };

            buttonNewPayment.Click += delegate
			{
                this.Recreate();
			};

        } // OnCreate

		void ParseReceipt(ZXing.Result result)
		{
            string[] paymentDetails = new string[] {};

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                paymentDetails = result.Text.Split(';');
                merchantID = paymentDetails[0].ToString();
                i004_trxn_amount = Convert.ToDouble(paymentDetails[1].ToString());
                referenceID = paymentDetails[2].ToString();
                currency = paymentDetails[3].ToString();

                paymentMsg += currency + ": " + i004_trxn_amount; // Amount
                paymentMsg += " to merchant with ID: " + merchantID; // Merchant
                paymentMsg += " Receipt reference: " + referenceID; // Receipt reference ID

                MakePayment();
			}
			else
				paymentMsg = "Scanning Canceled!";

            //textViewConsole.Text = paymentMsg;

		} // ParseReceipt

        private async void GetBalance()
		{
			apiCall = new APIClass();

            // ToDo: Dynamically build string
            string apiUrl = "Accounts/" + 
                            myDetails.ID + "/" +
                            myDetails.defaultCurrency
                            ;

            json = await apiCall.CallAPI(apiUrl);
            balance = Convert.ToDouble(json["balance"].ToString());

            textViewBalance.Text = "Available Balance: " + myDetails.defaultCurrency + balance.ToString();

            if (balance <= 0.00)
            {
                buttonScan.Enabled = false;
                textViewConsole.Text = "Cannot make payment - not enough available funds";
            }
            else
                buttonScan.Enabled = true; 

        } // Get Balance

        private void MakePayment ()
		{
            AlertDialog.Builder confirmPayment = new AlertDialog.Builder(this);
            confirmPayment.SetTitle("Make Payment?");
            confirmPayment.SetMessage(paymentMsg);
            confirmPayment.SetPositiveButton("Pay", async (sender, e) =>
            {
                apiCall = new APIClass();
                string apiUrl = "Transfers/" + 
                                myDetails.ID + "/" + 
                                merchantID + "/" + 
                                i004_trxn_amount.ToString() + "/" + 
                                referenceID + "/" + 
                                currency;
                json = await apiCall.CallAPI(apiUrl);
                textViewConsole.Text = json["status"].ToString();

                GetBalance(); // Update balance

            });

            confirmPayment.SetNegativeButton("Cancel", (sender, e) => 
            {
                textViewConsole.Text = "Transaction aborted";
                this.Recreate(); // Reset activity to try another payment
            });

            Dialog payDialog = confirmPayment.Create();
            payDialog.Show();
		}
	}
}
