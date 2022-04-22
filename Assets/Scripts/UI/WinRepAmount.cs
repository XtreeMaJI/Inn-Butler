using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WinRepAmount : MonoBehaviour
{
    public TMP_Text _RepAmountText;

    void Start()
    {
        SetAmountOfRep(LevelManager.WIN_REP_AMOUNT);
    }

    private void SetAmountOfRep(int NewNumOfRep)
    {
        _RepAmountText.SetText(NewNumOfRep.ToString());
    }

}
