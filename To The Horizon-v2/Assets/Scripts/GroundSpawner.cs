using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour

    
{
    public GameObject tiles;
    public Vector3 nextSPawnPoint;
    // Start is called before the first frame update
    public void SpawnTile()
    {
        GameObject temp = Instantiate(tiles, nextSPawnPoint, Quaternion.identity);
        nextSPawnPoint = temp.transform.GetChild(1).transform.position;

    }

    // Update is called once per frame
    void Start()
    {

        for (int i =0; i<15; i++)
        {
            SpawnTile();
        }
        
    }
}
