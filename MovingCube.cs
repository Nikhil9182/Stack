using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    public GameObject cube;
    [SerializeField]
    private float moveSpeed = 1f;
    public int flag1,flag2;

    private void OnEnable()
    {
        if(LastCube == null )
            LastCube = GameObject.Find("StartCube").GetComponent<MovingCube>();
        //wherever the script is attached the currentcube will be that
        CurrentCube = this;

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    internal void Stop()
    {
        moveSpeed = 0f;
        float hangover = GetHangover();
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangover) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }
        float direction = hangover > 0 ? 1f : -1f;
        if (MoveDirection == MoveDirection.Z)
        {
            SplitCubeOnZ(hangover, direction);
        }
        else
        {
            SplitCubeOnX(hangover, direction);
        }
        LastCube = this;
    }

    private float GetHangover()
    {
        if(MoveDirection == MoveDirection.Z) 
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;
        //as distance count from 0 so hangover is divided by 2 so that the size can be adjusted according to centre and newZScale determines the size of the cube after slice
        //and newZPosition is used to determine where to place the cube after sliced according to LastCube position
        float newXPosition = LastCube.transform.position.x + (hangover / 2f);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + (fallingBlockSize / 2f * direction);
        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover,float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;
        //as distance count from 0 so hangover is divided by 2 so that the size can be adjusted according to centre and newZScale determines the size of the cube after slice
        //and newZPosition is used to determine where to place the cube after sliced according to LastCube position
        float newZPosition = LastCube.transform.position.z + (hangover / 2f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + (fallingBlockSize / 2f * direction);
        SpawnDropCube(fallingBlockZPosition,fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition,float fallingBlockSize)
    {
        if(MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else if(MoveDirection == MoveDirection.X)
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }
        var fallcube = Instantiate(cube);
        Destroy(fallcube.gameObject, 1f);
    }

    void Update()
    {
        if(MoveDirection == MoveDirection.Z)
        {
            if (transform.position.z <= -1.5)
            {
                flag1 = 0;
            }
            if (transform.position.z >= 1.5)
            {
                flag1 = 1;
            }
            if (flag1 == 0)
            {
                transform.Translate(0f, 0f, moveSpeed * Time.deltaTime);
            }
            else if (flag1 == 1)
            {
                transform.Translate(0f, 0f, -moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.x <= -1.5)
            {
                flag2 = 0;
            }
            if (transform.position.x >= 1.5)
            {
                flag2 = 1;
            }
            if (flag2 == 0)
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f);
            }
            else if (flag2 == 1)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0f, 0f);
            }
        }
    }
}
