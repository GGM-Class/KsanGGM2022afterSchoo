using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
    // ���� �� ���¿��� �����ؾ��� �׼ǵ�
    public List<AIAction> actions = null;

    private AIBrain _brain;

    private void Awake()
    {
        _brain = transform.parent.parent.GetComponent<AIBrain>();
    }


    // ���� �� ���¿��� ���̰� ������ ���·��� ���� ����Ʈ��
    public List<AITransition> transitions = null;

    public void UpdateState()
    {
        //��� ���´� �� �����Ӹ��� �� �޼��带 �����Ѵ�.
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

            if (result) // ��� �����(����)�� �����ϴ� ����, Ʈ�������� �����Ѵ�
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
