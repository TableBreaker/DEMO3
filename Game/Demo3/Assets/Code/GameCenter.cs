using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }

    private void SetBackGroundColor(Color color)
    {
        Camera.main.backgroundColor = color;
    }

    public static GameCenter Instance { get; private set; }
}
