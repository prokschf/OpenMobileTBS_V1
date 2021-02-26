using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class MapCreator : MonoBehaviour
{
    public int Count;
    public float centerDistancePenalty = 1f;
    public int Width = 5000;
    public int Height = 5000;
    public string[,] Map { get; set; }
    public float growStrenthFalloff = 0.6f;
    public int SplatAmount = 2000;

    public bool Recreate;
    public int GrowChildrenAmount = 10;
    public List<GameObject> MapTiles { get; set; } = new List<GameObject>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Recreate)
        {
            Recreate = false;

            foreach (var mapTile in MapTiles)
            {
                GameObject.Destroy(mapTile);
            }
            MapTiles.Clear();
            Map = new string[Width, Height];
            GenerateMap();
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var mainCube = Instantiate(Resources.Load("Plane Water") as GameObject);
                    if (Map[x,y] == "D" || Map[x,y] == "d")
                    {
                        mainCube = Instantiate(Resources.Load("Plane") as GameObject);
                    }else if (Map[x, y] == "G" || Map[x, y] == "g")
                    {
                        mainCube = Instantiate(Resources.Load("Plane Green") as GameObject);
                    } else if (Map[x, y] == "I" || Map[x, y] == "i")
                    {
                        mainCube = Instantiate(Resources.Load("Plane Ice") as GameObject);
                    }
                    
                    mainCube.transform.position =
                        new Vector3(x, 0, y);
                    mainCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    MapTiles.Add(mainCube);
                }
            }
        }
    }

    public void GenerateMap()
    {
        void SetSplat(string terrain)
        {
            var x = Random.Range(0, Width - 1);
            var y = Random.Range(0, Height - 1);

            Map[x, y] = terrain;
            Grow(x, y, x, y, 8,terrain);
        }

        for (int i = 0; i < SplatAmount; i++)
        {
            SetSplat("D");
            SetSplat("G");
            SetSplat("I");
        }
    }

    public void Grow(int x, int y, int centerX, int centerY, float growStrength, string letter)
    {
        var distanceToCenterX = (centerX - x) * centerDistancePenalty;
        var distanceToCenterY = (centerY - y) * centerDistancePenalty;

        var targetX = x;
        var targetY = y;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            if (-distanceToCenterX + Random.Range(0.0f, 1.0f) < 0.5f)
            {
                targetX += 1;
            }
            else
            {
                targetX += -1;
            }
        }
        else
        {
            if (-distanceToCenterY + Random.Range(0.0f, 1.0f) > 0.5f)
            {
                targetY += 1;
            }
            else
            {
                targetY += -1;
            }
        }

        if (targetX >= Width || targetY >= Height || targetX < 0 || targetY < 0)
        {
            return;
        }
        if (Map[targetX, targetY] != letter.ToLower() && Map[targetX, targetY] != letter.ToUpper())
        {
            Map[targetX, targetY] = letter.ToLower();
            if (Random.Range(0.0f, 1.0f) < growStrength)
            {
                for (int i = 0; i < GrowChildrenAmount; i++)
                {
                    /*if (letter.ToLower() == "d")
                    {
                        growStrength *= Math.Abs(Height - targetY) / (Height * 3);
                    }*/
                    Grow(targetX, targetY, x, y, growStrength * growStrenthFalloff, letter.ToLower());
                }
            }
        }
    }
}
