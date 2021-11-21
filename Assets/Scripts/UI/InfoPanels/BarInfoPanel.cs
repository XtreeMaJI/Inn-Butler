using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarInfoPanel : MonoBehaviour
{
    private GameObject _ServantImage { get; set; }
    private TMP_Text _ServantXP;

    private void Awake()
    {
        _ServantImage = transform.Find("Servant").gameObject;

        _ServantXP = _ServantImage.transform.Find("ServantXP").GetComponent<TMP_Text>();
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

    public void activate_ServantImage()
    {
        _ServantImage.SetActive(true);
    }
}
