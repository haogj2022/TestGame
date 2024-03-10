using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public CameraController cameraController;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }
}
