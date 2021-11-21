using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KitchenInfoPanel : MonoBehaviour
{
    private GameObject _CookImage { get; set; }
    private GameObject _ServantImage { get; set; }

    private TMP_Text _CookXP;
    private TMP_Text _ServantXP;

    private Image _FoodBar;

    private void Awake()
    {
        _CookImage = transform.Find("Cook").gameObject;
        _ServantImage = transform.Find("Servant").gameObject;

        _CookXP = _CookImage.transform.Find("CookXP").GetComponent<TMP_Text>();
        _ServantXP = _ServantImage.transform.Find("ServantXP").GetComponent<TMP_Text>();

        _FoodBar = transform.Find("FoodBar").GetComponent<Image>();
    }

    public void set_CookXP(int Level, int CurXP, int MaxXP)
    {
        string NewText = "Уровень" + Level.ToString() + "\n" + CurXP.ToString();
        if(CurXP < MaxXP)
        {
            NewText = NewText + "/" + MaxXP.ToString() + " XP";
        }
        else
        {
            NewText = NewText + " XP";
        }
        _CookXP.SetText(NewText);
    }

    public void set_ServantXP(int Level, int CurXP, int MaxXP)
    {
        string NewText = "Уровень" + Level.ToString() + "\n" + CurXP.ToString();
        if (CurXP < MaxXP)
        {
            NewText = NewText + "/" + MaxXP.ToString() + " XP";
        }
        else
        {
            NewText = NewText + " XP";
        }
        _ServantXP.SetText(NewText);
    }

    public void activate_CookImage()
    {
        _CookImage.SetActive(true);
    }

    public void activate_ServantImage()
    {
        _ServantImage.SetActive(true);
    }

    public void set_FoodBar(float CurrentCookDegree)
    {
        _FoodBar.fillAmount = CurrentCookDegree;
    }

}
