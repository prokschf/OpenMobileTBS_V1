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
    public int Width = 512;
    public int Height = 512;
    public int HeightMapWidth = 4096;
    public int HeightMapHeight = 4096;
    public bool InstantiateTiles = false;
    public (string Terrain, int ID, float Elevation)[,] Map { get; set; }
    public float growStrenthFalloff = 0.6f;
    public int SplatAmount = 200;

    public float[,] HeightMap  { get; set; }
    public Terrain TerrainObject; 
    public bool Recreate;
    public int GrowChildrenAmount = 10;
    public float MaxElevation = 0.1f;
    public float MinElevation = -0.02f;
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
            Map = new (string, int, float)[Width, Height];
            GenerateMap();

            if (InstantiateTiles)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var mainCube = Instantiate(Resources.Load("Plane Water") as GameObject);
                        if (!String.IsNullOrWhiteSpace(Map[x, y].Terrain))
                        {
                            mainCube = Instantiate(Resources.Load("Plane") as GameObject);
                        }
                        /*}else if (Map[x, y] == "G" || Map[x, y] == "g")
                        {
                            mainCube = Instantiate(Resources.Load("Plane Green") as GameObject);
                        } else if (Map[x, y] == "I" || Map[x, y] == "i")
                        {
                            mainCube = Instantiate(Resources.Load("Plane Ice") as GameObject);
                        }*/

                        mainCube.transform.position =
                            new Vector3(x, Map[x, y].Elevation, y);
                        mainCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        MapTiles.Add(mainCube);
                    }
                }

            }

            HeightMap = new float[HeightMapWidth, HeightMapHeight];
            var HeightMapToTileRatioX = HeightMapWidth / Width;
            var HeightMapToTileRatioY = HeightMapHeight / Height;
            
            for (int x = 0; x < HeightMapWidth - HeightMapToTileRatioX; x++)
            {
                for (int y = 0; y < HeightMapHeight - HeightMapToTileRatioY; y++)
                {
                    var tlx = (int)(x / HeightMapToTileRatioX);
                    var tly = (int)(y / HeightMapToTileRatioY);
                    var xProp = (float)x / HeightMapToTileRatioX - tlx;
                    var yProp = (float)y / HeightMapToTileRatioY - tly;
                    var val = Map[tlx, tly].Elevation * (1 - xProp) * (1 - yProp) +
                              Map[tlx + 1, tly].Elevation * xProp * (1 - yProp) +
                              Map[tlx, tly + 1].Elevation * (1 - xProp) * yProp +
                              Map[tlx + 1, tly + 1].Elevation * xProp * yProp;
                    HeightMap[x, y] = val;
                }
            }

            TerrainObject.terrainData = new TerrainData()
            {
                heightmapResolution = HeightMapWidth,
                size = new Vector3(100, 2, 100)
            };
            TerrainObject.terrainData.SetHeights(0, 0, HeightMap);
        }
    }

    public void GenerateMap()
    {
        void SetSplat(string terrain, int id, float elevation)
        {
            var x = Random.Range(0, Width - 1);
            var y = Random.Range(0, Height - 1);

            Map[x, y] = (terrain, id, elevation);
            Grow(x, y, x, y, 8,terrain, elevation, id);
        }

        for (int i = 0; i < SplatAmount; i++)
        {
            SetSplat("{i}C", i, Random.Range(MinElevation, MaxElevation));
        }
    }

    public void Grow(int x, int y, int centerX, int centerY, float growStrength, string terrain, float elevation, int id)
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
        if (Map[targetX, targetY].Terrain != terrain.ToLower())
        {
            Map[targetX, targetY] = (terrain, id, elevation);
            if (Random.Range(0.0f, 1.0f) < growStrength)
            {
                for (int i = 0; i < GrowChildrenAmount; i++)
                {
                    /*if (letter.ToLower() == "d")
                    {
                        growStrength *= Math.Abs(Height - targetY) / (Height * 3);
                    }*/
                    Grow(targetX, targetY, x, y, growStrength * growStrenthFalloff, terrain.ToLower(), elevation, id);
                }
            }
        }
    }
}
