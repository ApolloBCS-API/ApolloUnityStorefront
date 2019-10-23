using UnityEngine;
using System.Collections;

public class FakeAccountController : MonoBehaviour {
	private static FakeAccountController _instance;

	public static FakeAccountController Instance {
		get {
			if (_instance == null) {
				GameObject dgManager = GameObject.Find ("Apollo_Controller") as GameObject;
				if (dgManager == null) {
					dgManager = GameObject.Instantiate (Resources.Load ("Prefabs/Apollo_Controller")) as GameObject;
				}
				_instance = dgManager.GetComponent<FakeAccountController> ();
			}	
			return _instance;
		}
	}
	
	public const string accountName = "Jimmy Nowel";
    private string _billingName = "";
    private int _livesAmount;
	private bool _isLoggedIn;
    public string accountPaymentToken = "None";
    private string _accountNumberMasked = "None";
    private string _accounExpiration = "None";

    void Start() {
		_livesAmount = 10;
		_isLoggedIn = true;
	}

    public string PaymentToken()
    {
        return accountPaymentToken;
    }

	public bool IsAccountOnline() {
		return _isLoggedIn;
	}

    public void UpdateBillingName(string CcName)
    {
        _billingName = CcName;
    }

    public void UpdateBillingExpiration(string CcExpiration)
    {
        _accounExpiration = CcExpiration;
    }

    public void UpdateMaskedCard(string MaskedCc)
    {
        _accountNumberMasked = MaskedCc;
    }

    public void Login() {
		_isLoggedIn = true;
	}
	
	public void Logout() {
		_isLoggedIn = false;
	}

	public int GetLives() {
		return _livesAmount;
	}

    public string GetCardHolderName()
    {
        return _billingName;
    }

    public string GetMaskedCardNumber()
    {
        return _accountNumberMasked;
    }

    public string GetCardExpiration()
    {
        return _accounExpiration;
    }

    public bool IncreaseLives(int amount) {
		if (amount <= 0) {
			return false;
		}
		
		if (_isLoggedIn) {
			_livesAmount += amount;
			return true;
		} else {
			return false;
		}
	}
	
	public bool DecreaseLives(int amount) {
		if (amount <= 0) {
			return false;
		}
		
		if (_isLoggedIn) {
			int valueAfter = _livesAmount - amount;
			if (valueAfter < 0) {
				return false;
			} else {
				_livesAmount -= amount;
				return true;	
			}
		} else {
			return false;	
		}
	}
}
