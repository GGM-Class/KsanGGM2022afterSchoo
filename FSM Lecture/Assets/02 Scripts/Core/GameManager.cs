using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Transform _playerTrm;
    public Transform PlayerTrm => _playerTrm;

    private void Awake()
    {
        _playerTrm = GameObject.Find("Player").transform;

        if(Instance != null) // 예외 처리
        {
            Debug.LogError("Multiple Gamemanager is running!");
        }
        Instance = this;
    }

    
}
