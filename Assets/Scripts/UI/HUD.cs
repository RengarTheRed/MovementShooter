using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Interact & Timer text elements
    public TMP_Text _interactText;
    public TMP_Text _timerText;
    
    //Ammo UI Components
    public TMP_Text _ammoText;
    public Slider _ammoSlider;
    
    //HP UI Components
    public TMP_Text _hpText;
    public Slider _hpSlider;

    public void SetupHUD(int maxHP, int maxAmmo)
    {
        //Ammo setup
        _ammoText.SetText(maxAmmo.ToString());
        _ammoSlider.maxValue = maxAmmo;
        _ammoSlider.value = maxAmmo;
        
        //HP setup
        _hpText.SetText(maxHP.ToString());
        _hpSlider.maxValue = maxHP;
        _hpSlider.value = maxHP;
    }

    public void UpdateInteractText(string newText)
    {
        _interactText.SetText(newText);
    }

    public void UpdateAmmo(int newAmmo)
    {
        _ammoText.SetText(newAmmo.ToString());
        _ammoSlider.value = newAmmo;
    }
    public void UpdateHP(int newHP)
    {
        _hpText.SetText(newHP.ToString());
        _hpSlider.value = newHP;
    }

    public void UpdateTimer(float newTime)
    {
        _timerText.SetText(newTime.ToString("F0"));
    }
}