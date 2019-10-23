using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine.UI;

public class ZiftOneTimeCharge : MonoBehaviour
{
    public void Main()
    {

    }

    public GameObject shoppingCartUI;
    public GameObject checkoutCartUI;
    public GameObject successUI;
    public GameObject failUI;
    public GameObject ContinueNav;
    public Text inputCardNumber;
    public Text inputExpDate;
    public Text inputCVV;
    public Text inputEmail;
    public Text inputCardName;
    public Text totalPay;

    protected static int TIMEOUT = 1 * 60 * 1000;

    string transactionAmount;

    //Zift Credentials
    string userName = ""; //Add your Zift Username here
    string password = ""; //Add your Zift Password here
    string accountId = ""; //Add your Zift account Id here

    private bool storePaymentToken = true;
    string requestResponse;

    void OnEnable()
    {
        transactionAmount = (CartManager.Instance.totalMoney * 100).ToString();
        totalPay.text = CartManager.Instance.totalMoney.ToString("C2") + " USD";
    }

    public void ChargeSingle()
    {
        if (FakeAccountController.Instance.IsAccountOnline())
        {
            if (FakeAccountController.Instance.accountPaymentToken != "None")
            {
                //Go to https://www.apollobcs.com/getstarted/  to get started with Ziftpay Apollo API
                requestResponse = SendPOST("https://sandbox-secure.ziftpay.com/gates/xurl?", //Add the Zift production URL here (https://secure.ziftpay.com/gates/xurl?)
                    "requestType=sale"
                    + "&userName=" + userName
                    + "&password=" + password
                    + "&accountId=" + accountId 
                    + "&amount=" + transactionAmount
                    + "&accountType=R"
                    + "&transactionIndustryType=RE"
                    + "&holderType=P"
                    + "&holderName=" + FakeAccountController.Instance.GetCardHolderName()
                    + "&accountAccessory=" + FakeAccountController.Instance.GetCardExpiration()
                    + "&token=" + FakeAccountController.Instance.accountPaymentToken
                );
            }
            else if (storePaymentToken)
            {
                //Go to https://www.apollobcs.com/getstarted/  to get started with Ziftpay Apollo API
                requestResponse = SendPOST("https://sandbox-secure.ziftpay.com/gates/xurl?", //Add the Zift production URL here (https://secure.ziftpay.com/gates/xurl?)
                    "requestType=tokenization"
                    + "&userName=" + userName
                    + "&password=" + password
                    + "&accountId=" + accountId
                    + "&accountType=R" 
                    + "&transactionIndustryType=RE"
                    + "&holderType=P"
                    + "&holderName=" + inputCardName.text
                    + "&accountNumber=" + inputCardNumber.text
                    + "&accountAccessory=" + inputExpDate.text
                );

                successUI.GetComponent<SuccessUI>().SetAccountToken(requestResponse);
                requestResponse = SingleSale();
            }
            else
            {
                requestResponse = SingleSale();
            }
        }
        else
        {
            shoppingCartUI.SetActive(false);
            checkoutCartUI.SetActive(false);
            //transform.parent.gameObject.SetActive(false);
            failUI.SetActive(true);
            failUI.GetComponent<FailedUI>().SetErrorMessage("You must login before progess any purchase");
            return;
        }
        
        if (requestResponse.Contains("providerResponseMessage=Approved"))
        {
            shoppingCartUI.SetActive(false);
            checkoutCartUI.SetActive(false);
            //transform.parent.gameObject.SetActive(false);
            CartManager.Instance.ProgessPurchase();
            successUI.GetComponent<SuccessUI>().UpdateTransactionDetails(requestResponse, transactionAmount);
            successUI.SetActive(true);
            ContinueNav.GetComponent<Button>().interactable = true;
        }
        else
        {
            shoppingCartUI.SetActive(false);
            checkoutCartUI.SetActive(false);
            //transform.parent.gameObject.SetActive(false);
            failUI.SetActive(true);
            failUI.GetComponent<FailedUI>().SetErrorMessage("Testing");
        }
    }

    protected string SingleSale()
    {
        //Go to https://www.apollobcs.com/getstarted/  to get started with Ziftpay Apollo API
        requestResponse = SendPOST("https://sandbox-secure.ziftpay.com/gates/xurl?", //Add the Zift production URL here (https://secure.ziftpay.com/gates/xurl?)
            "requestType=sale"
            + "&userName=" + userName
            + "&password=" + password
            + "&accountId=" + accountId
            + "&amount=" + transactionAmount
            + "&accountType=R"
            + "&transactionIndustryType=RE"
            + "&holderType=P"
            + "&holderName=" + inputCardName.text
            + "&accountNumber=" + inputCardNumber.text
            + "&accountAccessory=" + inputExpDate.text
        );

        return requestResponse;
    }

    public static string SendPOST(String url, String data)
    {
        WebRequest request = WebRequest.Create(url);

        byte[] byteArray = Encoding.UTF8.GetBytes(data);
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "POST";
        request.Timeout = TIMEOUT;
        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        try
        {
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            return responseFromServer;
        }
        catch (WebException e)
        {
            return e.Message;
        }
    }

    public void SetTokinizationChecked()
    {
        storePaymentToken = !storePaymentToken;
    }

    public void CheckoutClose()
    {
        checkoutCartUI.SetActive(false);
    }
}
