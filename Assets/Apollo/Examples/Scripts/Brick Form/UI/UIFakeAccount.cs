﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFakeAccount : MonoBehaviour {
	public GameObject dropMenuAccount;
	public Text txtAccount;

	void Start() {
		SetTextAccountStatus(FakeAccountController.Instance.IsAccountOnline());
	}

	private void SetTextAccountStatus(bool isLogin) {
		if(isLogin) {
			txtAccount.text = FakeAccountController.accountName;
		} else {
			txtAccount.text = "Login";
		}
	}

	public void OnToggleLoginDropbox(bool isNotShow) {
		dropMenuAccount.SetActive(isNotShow);
	}

}
