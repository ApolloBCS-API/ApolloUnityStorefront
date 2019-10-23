using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaskedCard : MonoBehaviour
{
    public Text txtMaskedCardNumber;

    // Update is called once per frame
    void Update()
    {
        txtMaskedCardNumber.text = FakeAccountController.Instance.GetMaskedCardNumber();
    }
}
