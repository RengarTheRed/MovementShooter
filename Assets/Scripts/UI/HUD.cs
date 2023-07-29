using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TMP_Text _interactText;
    public TMP_Text _ammoText;

    public void UpdateInteractText(string newText)
    {
        _interactText.SetText(newText);
    }

    public void UpdateAmmoText(int newCount)
    {
        Debug.Log(newCount);
        _ammoText.SetText(newCount.ToString());
    }
}