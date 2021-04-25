using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject TilePrefab;
    public int Width, Height;


    // Start is called before the first frame update
    void Start()
    {
        //GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GenerateLevel()
    {
        for(int i = 0; i < Width; i++)
        {
            for (int j = 1; j < Height; j++)
            {
                GameObject go = GameObject.Instantiate(TilePrefab, new Vector3(i, -j, 0), Quaternion.identity);
                go.transform.SetParent(this.transform);

                TileAgent tile = go.GetComponent<TileAgent>();
                tile.X = i;
                tile.Y = j;
                tile.Value = 0;
            }
        }
    }

}
