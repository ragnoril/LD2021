using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] TilePrefabs;
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
        for (int j = 1; j < Height; j++)
        {
            int NoneTier = Mathf.Clamp(9 - j, 1, 9);
            int LowTier = NoneTier + Mathf.Clamp(15 - j, 5, 15);
            int MidLowTier = LowTier + Mathf.Clamp(45 - j, 25, 45);
            int MidTier = MidLowTier + Mathf.Clamp(25 + j, 25, 45);
            int MidHighTier = MidTier + Mathf.Clamp(5 + j, 1, 9);
            int HighTier = MidHighTier + Mathf.Clamp(1 + j, 1, 9);

            for (int i = 0; i < Width; i++)
            {
                int tileId = 0;
                int diceRnd = Random.Range(0, 100);

                if (diceRnd <= NoneTier)
                {
                    tileId = 0;
                }
                else if (diceRnd <= LowTier)
                {
                    tileId = 1;
                }
                else if (diceRnd <= MidLowTier)
                {
                    tileId = 2;
                }
                else if (diceRnd <= MidTier)
                {
                    tileId = 3;
                }
                else if (diceRnd <= MidHighTier)
                {
                    tileId = 4;
                }
                else if (diceRnd <= HighTier)
                {
                    tileId = 5;
                }

                GameObject go = GameObject.Instantiate(TilePrefabs[tileId], new Vector3(i, -j, 0), Quaternion.identity);
                go.transform.SetParent(this.transform);

                TileAgent tile = go.GetComponent<TileAgent>();
                tile.X = i;
                tile.Y = j;
                //tile.Value = 0;

                

            }
        }
    }

}
