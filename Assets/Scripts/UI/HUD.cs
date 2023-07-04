using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TMP_Text _interactText;

    public void UpdateInteractText(string newText)
    {
        _interactText.SetText(newText);
    }
}
