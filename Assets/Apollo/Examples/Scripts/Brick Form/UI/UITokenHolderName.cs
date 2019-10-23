using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITokenHolderName : MonoBehaviour
{
    public Text txtCardholderName;

    // Update is called once per frame
    void Update()
    {
        txtCardholderName.text = FakeAccountController.Instance.GetCardHolderName();
    }
}
