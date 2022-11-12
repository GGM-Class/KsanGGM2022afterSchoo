using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelAction : AIAction
{
    

    public override void TakeAction()
    {
        _brain.Move(Vector2.zero, _brain.target.position);
        Debug.Log("현재 Idle액션 실행중");


    }
}
