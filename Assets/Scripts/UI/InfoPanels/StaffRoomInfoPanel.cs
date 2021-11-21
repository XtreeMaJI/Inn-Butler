using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaffRoomInfoPanel : MonoBehaviour
{
    private GameObject _HousemaidImage { get; set; }
    private TMP_Text _HousemaidXP;

    private void Awake()
    {
        _HousemaidImage = transform.Find("Housemaid").gameObject;

        _HousemaidXP = _HousemaidImage.transform.Find("HousemaidXP").GetComponent<TMP_Text>();
    }

    public void set_HousemaidXP(int Level, int CurXP, int MaxXP)
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
        _HousemaidXP.SetText(NewText);
    }

    public void activate_HousemaidImage()
    {
        _HousemaidImage.SetActive(true);
    }
}
