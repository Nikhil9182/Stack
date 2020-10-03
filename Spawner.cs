using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    private float length = 0.1f;
    private float flag = 0;
    [SerializeField]
    private MoveDirection movedirection;
    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        float x = movedirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
        float z = movedirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;
        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("StartCube"))
            {
                cube.transform.position = new Vector3(x,
                    MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                    z);
            }
            else if (flag == 0)
            {
                cube.transform.position = new Vector3(x, transform.position.y, z);
                flag = 1f;
            }
            else if (flag == 1f)
            {
                cube.transform.position = new Vector3(x, transform.position.y + length, z);
                length += 0.1f;
            }
        cube.MoveDirection = movedirection;
        ScoreManager.Points += 1;
    }
}
