using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateMap : MonoBehaviour
{
    public static CreateMap Instance;
    
    [SerializeField] private int width, height;
    [SerializeField] private GameObject walls, floor;
    [SerializeField] private Transform map;

    private TileData[,] mapArray;
    void Start()
    {
        Instance = this;
        
        mapArray = new TileData[width-2, height-2];
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

    public TileData GetTileData(int x, int y)
    {
        return mapArray[x, y];
    }
    
}
