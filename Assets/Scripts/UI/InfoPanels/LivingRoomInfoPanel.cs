using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LivingRoomInfoPanel : MonoBehaviour
{
    private TMP_Text _DaysBeforeLeave;
    private Image _VisSprite;
    private Image _CleanBar;
    private Image _FoodBar;
    private Image _FunBar;
    private Image _HappinessBar;

    private void Start()
    {
        _DaysBeforeLeave = transform.Find("DaysBeforeLeave").GetComponent<TMP_Text>();
        _VisSprite = transform.Find("VisitorSprite").GetComponent<Image>();
        _CleanBar = transform.Find("CleanBar").GetComponent<Image>();
        _FoodBar = transform.Find("FoodBar").GetComponent<Image>();
        _FunBar = transform.Find("FunBar").GetComponent<Image>();
        _HappinessBar = transform.Find("HappinessBar").GetComponent<Image>();
    }

    public void set_VisSprite(Sprite newSprite = null)
    {
        if(_VisSprite == null)
        {
            Start();
        }
        _VisSprite.sprite = newSprite;
    }

    public void set_CleanBarFill(float fill)
    {
        _CleanBar.fillAmount = fill;
    }

    public void set_FoodBarFill(float fill)
    {
        _FoodBar.fillAmount = fill;
    }

    public void set_FunBarFill(float fill)
    {
        _FunBar.fillAmount = fill;
    }

    public void set_HappinessBarFill(float fill)
    {
        _HappinessBar.fillAmount = fill;
    }

    public void set_DaysBeforeLeave(int numOfDays)
    {
        string NewText = "Дней до отъезда: " + numOfDays.ToString();
        _DaysBeforeLeave.SetText(NewText);
    }

}
 

