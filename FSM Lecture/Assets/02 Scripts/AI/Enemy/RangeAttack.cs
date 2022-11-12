using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : EnemyAttack
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _playTime = 1f;
    private Action<bool> callback = null;   

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = _brain.GetComponent<SpriteRenderer>();
    }

    public override void Attack(Action<bool> Callback)
    {
        _spriteRenderer.color = Color.red;
        this.callback = Callback;
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_playTime);
        _spriteRenderer.color = Color.white;
        callback(true);
    }
}
