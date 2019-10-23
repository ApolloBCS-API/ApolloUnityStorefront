using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class SuccessUI : MonoBehaviour {
	public UINavigation navHandler;
	public GameObject searchButton;
	public GameObject btnCart;
	public Text txtReceipt;
	public Text txtOrderTotal;
    public Text txtReceiptDate;
    public Text txtAccountNumber;
    public Text txtPaymentMethod;

	void OnEnable() {
		searchButton.SetActive(false);
		btnCart.SetActive(false);
		navHandler.gameObject.SetActive (false);
	}

	public void OnGoBackToShopping(Button pSender) {
		gameObject.SetActive(false);
		navHandler.gameObject.SetActive(true);
		searchButton.SetActive(true);
		btnCart.SetActive(true);
		navHandler.SwitchTab(pSender);
	}

	public void UpdateTransactionDetails(string receipt, string total) {
        double tempTotal;
        List<string> receiptBreakdown = receipt.Split('&').ToList();
        string paymentMethod = receiptBreakdown.Where(w => w.Contains("extendedAccountType")).AsEnumerable().SingleOrDefault().Replace("extendedAccountType=", "");
        double.TryParse(total, out tempTotal);
        txtOrderTotal.text = "$" + (tempTotal / 100).ToString() + " USD";
        txtAccountNumber.text = receiptBreakdown.Where(w => w.Contains("accountId")).AsEnumerable().SingleOrDefault().Replace("accountId=", "");
        txtReceiptDate.text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        txtReceipt.text = receiptBreakdown.Where(w => w.Contains("transactionId")).AsEnumerable().SingleOrDefault().Replace("transactionId=", ""); ;
        txtPaymentMethod.text = PaymentMethodDict(paymentMethod);
	}

    public void SetAccountToken(string receipt)
    {
        string accountToken; 
        string CcNumberMasked;
        string CcHolderName;
        string CcExpiration;
        List<string> receiptBreakdown = receipt.Split('&').ToList();
        accountToken = receiptBreakdown.Where(w => w.Contains("token=")).AsEnumerable().SingleOrDefault().Replace("token=", "");
        CcNumberMasked = receiptBreakdown.Where(w => w.Contains("accountNumberMasked=")).AsEnumerable().SingleOrDefault().Replace("accountNumberMasked=", "");
        CcHolderName = receiptBreakdown.Where(w => w.Contains("holderName=")).AsEnumerable().SingleOrDefault().Replace("holderName=", "");
        CcExpiration = receiptBreakdown.Where(w => w.Contains("accountAccessory=")).AsEnumerable().SingleOrDefault().Replace("accountAccessory=", "");
        FakeAccountController.Instance.accountPaymentToken = accountToken;
        FakeAccountController.Instance.UpdateMaskedCard(CcNumberMasked);
        FakeAccountController.Instance.UpdateBillingName(CcHolderName);
        FakeAccountController.Instance.UpdateBillingExpiration(CcExpiration);
    }

    private string PaymentMethodDict(string payment)
    {
        Dictionary<string, string> cardTypeDictionary = new Dictionary<string, string>()
        {
            {"VC", "Visa credit card"},
            {"VD", "Visa debit card"},
            {"MC", "MasterCard credit card"},
            {"MD", "MasterCard debit card"},
            {"DC", "Discover credit card"},
            {"DD", "Discover debit card"},
            {"AC", "AmEx credit card"},
            {"NC", "Dinners credit card"},
            {"BD", "Bank debit Card"},
            {"BC", "Checking account"},
            {"BS", "Savings account"},
            {"", ""}
        };

        return cardTypeDictionary[payment] ?? "";
    }
}
