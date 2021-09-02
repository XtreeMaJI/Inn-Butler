using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    private int Reputation = 0;
    private LevelManager _LM;
    private ReputationAmount _RepAmount;

    private void Start()
    {
        _LM = FindObjectOfType<LevelManager>();
        _RepAmount = FindObjectOfType<ReputationAmount>();
    }

    public void increase_reputation(int RepAmount)
    {
        Reputation += RepAmount;
        if(_LM != null)
        {
            _LM.RepSelf = Reputation;
            _RepAmount.set_Rep_Count(Reputation);
        }
    }

}
