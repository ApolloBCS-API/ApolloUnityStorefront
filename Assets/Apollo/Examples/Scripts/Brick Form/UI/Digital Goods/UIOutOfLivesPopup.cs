using UnityEngine;
using UnityEngine.UI;

public class UIOutOfLivesPopup : MonoBehaviour {
	public UINavigation navHandler;

	public void OnClose() {
		gameObject.SetActive (false);
	}

	public void BuyMoreLives(Button pSender) {
		OnClose();
		navHandler.SwitchTab(pSender);
	}
}
