using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILivesBalance : MonoBehaviour {
	public GameObject popupOutOfLives;
	public Text txtLives;

	void Update() {
        txtLives.text = FakeAccountController.Instance.GetLives().ToString();
	}

	public void UpdateTextBalance() {
        txtLives.text = FakeAccountController.Instance.GetLives().ToString();
	}

	public void OnClickWhenHaveNoLives() {
		if(FakeAccountController.Instance.GetLives() == 0) {
			popupOutOfLives.SetActive(true);
		}
	}
}
