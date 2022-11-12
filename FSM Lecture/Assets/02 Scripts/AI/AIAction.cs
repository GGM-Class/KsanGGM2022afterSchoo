using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 클래스
public abstract class AIAction : MonoBehaviour
{
    protected AIBrain _brain;

    private void Awake()
    {
        _brain = transform.parent.parent.GetComponent<AIBrain>();
    }

    public abstract void TakeAction(); // 추상 메서드


}
