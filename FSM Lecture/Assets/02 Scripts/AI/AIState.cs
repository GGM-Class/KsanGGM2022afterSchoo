using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
    // 내가 이 상태에서 수행해야할 액션들
    public List<AIAction> actions = null;

    private AIBrain _brain;

    private void Awake()
    {
        _brain = transform.parent.parent.GetComponent<AIBrain>();
    }


    // 내가 이 상태에서 전이가 가능한 상태로의 전이 리스트들
    public List<AITransition> transitions = null;

    public void UpdateState()
    {
        //모든 상태는 매 프레임마다 이 메서드를 실행한다.
        foreach(AIAction a in actions)
        {
            a.TakeAction();
        }

        foreach(AITransition t in transitions)
        {
            bool result = false;

            foreach(AIDecision d in t.decisions)
            {
                result = d.MakeADecision();
                if (!result) break;
            }

            if (result) // 모든 디시전(조건)을 만족하는 상태, 트랜지션을 전이한다
            {
                _brain.ChangeToState(t.positiveResult);
            }
            else
            {
                if(t.negativeResult != null)
                {
                    _brain.ChangeToState(t.negativeResult);
                }
            }
        }
    }
}
