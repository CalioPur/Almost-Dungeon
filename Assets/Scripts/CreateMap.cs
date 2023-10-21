using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateMap : MonoBehaviour
{
    public int width, height;
    public GameObject walls, floor;
    public Transform map;

    public TileData[,] mapArray;
    void Start()
    {
        mapArray = new TileData[width, height];
        for(int i =0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                Vector3 pos = new Vector3(i-((float)(width-1)/2),0,j- (float)(height-1)/2); //pour centrer le tout
                if(i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    Instantiate( walls, pos, walls.transform.rotation, map); //verifie si on est sur un bord
                }
                else
                {
                    mapArray[i - 1, j - 1] = Instantiate(floor, pos, walls.transform.rotation, map).GetComponent<TileData>(); //verifie si on est sur un bord
                }


            }
        }
    }

    
}
