using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : MonoBehaviour
{
    public Room CurRoom;
    
    public LevelManager.TypeOfVisitor VisitorType { get; private set; }

    public int LeaseTerm; //Время, на которое посетитель займёт комнату

    protected void Start()
    {
        LeaseTerm = Random.Range(1, 8);
    }

}
