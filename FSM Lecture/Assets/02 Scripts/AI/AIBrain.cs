using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackData
{
    public EnemyAttack atk;
    public Action<bool> action;
    public float coolTime;
}

public class AIBrain : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementCommend;


    [SerializeField]
    private AIState _currentState;
    private AIStateInfo _stateInfo;
    private AgentMovement _movement;

    private Dictionary<SkillName, EnemyAttackData> _attackDictinary = new Dictionary<SkillName, EnemyAttackData>();

    public Transform target = null;

    private void Awake()
    {
        _stateInfo = transform.Find("AI").GetComponent<AIStateInfo>();
        _movement = GetComponent<AgentMovement>();
    }
    private void Start()
    {
        target = GameManager.Instance.PlayerTrm;
        MakeAttackType();
    }

    protected virtual void Update()
    {
        _currentState.UpdateState();

        if(_stateInfo.MeleeCool > 0)
        {
            _stateInfo.MeleeCool -= Time.deltaTime;
            if (_stateInfo.MeleeCool < 0) _stateInfo.MeleeCool = 0;
        }
        if(_stateInfo.RangeCool > 0)
        {
            _stateInfo.RangeCool -= Time.deltaTime;
            if (_stateInfo.RangeCool < 0) _stateInfo.RangeCool = 0;
        }
    }

    private void MakeAttackType()
    {
        Transform atkTrm = transform.Find("AttackType");
        EnemyAttackData rangeAttack = new EnemyAttackData
        {
            atk = atkTrm.GetComponent<RangeAttack>(),
            action = (value) =>
            {
                _stateInfo.IsAttack = false;
                _stateInfo.IsRange = false;
            },
            coolTime = 3f
        };
        EnemyAttackData meleeAttack = new EnemyAttackData
        {
            atk = atkTrm.GetComponent<MeleeAttack>(),
            action = (value) =>
            {
                _stateInfo.IsAttack = false;
                _stateInfo.IsMelee = false;
            },
            coolTime = 0.5f
        };


        _attackDictinary.Add(SkillName.Range, rangeAttack);
        _attackDictinary.Add(SkillName.Melee, meleeAttack);
    }


    public void ChangeToState(AIState nextState)
    {
        _currentState = nextState;
    }


    public virtual void Attack(SkillName skillName)
    {
        if (_stateInfo.IsAttack)
        {
            return;
        }
        EnemyAttackData atkData = null;
        FieldInfo fInfo = typeof(AIStateInfo).GetField($"{skillName.ToString()}Cool", BindingFlags.Public | BindingFlags.Instance);

        if((float)fInfo.GetValue(_stateInfo) > 0)
        {
            return; // 쿨타임 존재시 공격 불가
        }
        FieldInfo fInfoBool = typeof(AIStateInfo).GetField($"Is{skillName.ToString()}", BindingFlags.Public | BindingFlags.Instance);

        if (_attackDictinary.TryGetValue(skillName, out atkData))
        {
            _movement.StopImmediatly();
            _stateInfo.IsAttack = true;
            fInfoBool.SetValue(_stateInfo, true); //해당 스킬 공격중으로 셋팅
            fInfo.SetValue(_stateInfo, atkData.coolTime);
            atkData.atk.Attack(atkData.action);
        }

    }
    public void Move(Vector2 direction, Vector2 targetPos)
    {
        OnMovementCommend?.Invoke(direction);
    }
}
