using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : AIAction
{
    public override void TakeAction()
    {
        //목표를 쫒아가도록 실행
        Debug.Log("현재 chase액션 실행중");

        Vector2 direction = _brain.target.position - transform.position;

        _brain.Move(direction.normalized, _brain.target.position);
    }
}
