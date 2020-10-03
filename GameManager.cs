using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Spawner[] spawners;
    public int index = 1;
    private Spawner currentSpawner;


    private void Awake()
    {
        spawners = GameObject.FindObjectsOfType<Spawner>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (MovingCube.CurrentCube != null)
                    MovingCube.CurrentCube.Stop();

                index = index == 0 ? 1 : 0;
                currentSpawner = spawners[index];

                currentSpawner.SpawnCube();
            }
        }
    }
}
