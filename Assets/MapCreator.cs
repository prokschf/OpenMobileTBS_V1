using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MapTile
{
    public Vector2 Center;
    public Vector2 TopLeft;
    public Vector2 BottomRight;
    public string Terrain;
    public int ID;
    public float Elevation;
}

public class MapCreator : MonoBehaviour
{
    public int Count;
    public float centerDistancePenalty = 1f;
    public int Width = 512;
    public int Height = 512;
    public int HeightMapWidth = 4096;
    public int HeightMapHeight = 4096;
    public int AlphaMapResolution = 512;
    public bool InstantiateTiles = false;
    public int BrushStrokeCount = 10;
    public Transform Ocean;
    public bool DrawGrid = true;
    public float BlendThreshold = 0.8f;
    public (string Terrain, int ID, float Elevation)[,] Map { get; set; }
    public float growStrenthFalloff = 0.6f;
    public int SplatAmount = 200;
    
    private Dictionary<string, Texture2D[]> Brushes = new Dictionary<string, Texture2D[]>();
    
    public Dictionary<int, MapTile> MapTiles = new Dictionary<int, MapTile>();
    
    public float[,] HeightMap  { get; set; }
    public float[,,] AlphaMaps  { get; set; }
    
    public Terrain TerrainObject; 
    public bool Recreate;
    public int GrowChildrenAmount = 10;
    public float MaxElevation = 0.3f;
    public float MinElevation = 0;
    public List<GameObject> MapSquares { get; set; } = new List<GameObject>();
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DrawGrid)
        {
            var gtr = 100.0f / Width;
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    var xP = x * gtr;
                    var yP = y * gtr;
                    if (Map?[y, x] == null)
                    {
                        continue;
                        
                    }
                    
                    /*if (Map?[y, x].Terrain != null && Map[y, x].Terrain != "")
                    {
                        Debug.DrawLine(new Vector3(xP + gtr, .1f, yP), new Vector3(xP + gtr, .1f, yP + gtr), Color.red);
                        Debug.DrawLine(new Vector3(xP, .1f, yP + gtr), new Vector3(xP + gtr, .1f, yP + gtr), Color.red);
                    }
                    else
                    {
                        Debug.DrawLine(new Vector3(xP + gtr, .1f, yP), new Vector3(xP + gtr, .1f, yP + gtr));
                        Debug.DrawLine(new Vector3(xP, .1f, yP + gtr), new Vector3(xP + gtr, .1f, yP + gtr));
                    }*/

                    if (Map[y, x].ID != Map[y + 1, x].ID)
                    {
                        Debug.DrawLine(new Vector3(xP, .12f, yP + gtr), new Vector3(xP + gtr, .12f, yP + gtr), Color.red);
                    }
                    
                    if (Map[y, x].ID != Map[y, x + 1].ID)
                    {
                        Debug.DrawLine(new Vector3(xP + gtr, .12f, yP), new Vector3(xP + gtr, .12f, yP + gtr), Color.red);
                    }
                }
            }
        }
        
        if (Recreate)
        {
            Recreate = false;

            foreach (var mapTile in MapSquares)
            {
                GameObject.Destroy(mapTile);
            }
            MapTiles.Clear();
            MapSquares.Clear();
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
                        MapSquares.Add(mainCube);
                    }
                }

            }

            HeightMap = new float[HeightMapWidth, HeightMapHeight];
            
                        TerrainObject.terrainData = new TerrainData()
            {
                heightmapResolution = HeightMapWidth,
                size = new Vector3(100, 2, 100)
            };
            TerrainObject.terrainData.terrainLayers = new[]
            {
                new TerrainLayer()
                {
                    diffuseTexture = Resources.Load("Terrain Assets/TerrainTextures/rock_albedo") as Texture2D,
                    normalMapTexture = Resources.Load("Terrain Assets/TerrainTextures/rock_normal") as Texture2D,
                    maskMapTexture = Resources.Load("Terrain Assets/TerrainTextures/rock_mask") as Texture2D,
                    name = "Rock",
                    normalScale = 1,
                    tileSize = new Vector2(0.2f, 0.2f)
                },
                new TerrainLayer()
                {
                    diffuseTexture = Resources.Load("Terrain Assets/TerrainTextures/snow_albedo") as Texture2D,
                    normalMapTexture = Resources.Load("Terrain Assets/TerrainTextures/snow_normal") as Texture2D,
                    maskMapTexture = Resources.Load("Terrain Assets/TerrainTextures/snow_mask") as Texture2D,
                    name = "Snow",
                    tileSize = new Vector2(0.2f, 0.2f),
                    normalScale = 1
                },
                new TerrainLayer()
                {
                    diffuseTexture = Resources.Load("Terrain Assets/TerrainTextures/moss_albedo") as Texture2D,
                    normalMapTexture = Resources.Load("Terrain Assets/TerrainTextures/moss_normal") as Texture2D,
                    maskMapTexture = Resources.Load("Terrain Assets/TerrainTextures/moss_mask") as Texture2D,
                    name = "Moss",
                    normalScale = 1,
                    tileSize = new Vector2(0.2f, 0.2f)
                },
                new TerrainLayer()
                {
                    diffuseTexture = Resources.Load("GrassClumps_albedo") as Texture2D,
                    normalMapTexture = Resources.Load("GrassClumbs_normal") as Texture2D,
                    //maskMapTexture = Resources.Load("Terrain Assets/TerrainTextures/sand_mask") as Texture2D,
                    name = "Grass",
                    normalScale = 1,
                    tileSize = new Vector2(0.2f, 0.2f)
                } ,
                new TerrainLayer()
                {
                    diffuseTexture = Resources.Load("NaturalTilingTextures/CraterLakeRed/T_CraterLakeRed_BC") as Texture2D,
                    normalMapTexture = Resources.Load("NaturalTilingTextures/CraterLakeRed/T_CraterLakeRed_N") as Texture2D,
                    //maskMapTexture = Resources.Load("NaturalTilingTextures/CraterLakeRed/T_CraterLakeRed_BC") as Texture2D,
                    normalScale = 1,
                    name = "CraterLakeRed",
                    tileSize = new Vector2(0.2f, 0.2f)
                },
            };
            AlphaMaps = new float[AlphaMapResolution, AlphaMapResolution, TerrainObject.terrainData.terrainLayers.Length];
            TerrainObject.terrainData.alphamapResolution = AlphaMapResolution;
        

            
            //ApplyTerrainBrushes();
            ElevateTiles();

            TerrainObject.terrainData.SetAlphamaps(0, 0, AlphaMaps);
            TerrainObject.terrainData.SetHeights(0, 0, HeightMap);
            
        }
    }

    public void ElevateTiles()
    {
        var MapToHeightMap = (float)Width / (float)HeightMapWidth;
        for (int x = 0; x < HeightMapWidth; x++)
        {
            for (int y = 0; y < HeightMapHeight; y++)
            {
                var tlx = (int)(x * MapToHeightMap);
                var tly = (int)(y * MapToHeightMap);
                if (tlx <= 1 || tly <= 1 || tlx >= Width - 1 || tly >= Height - 1)
                {
                    continue;
                }

                var xProp = (x * MapToHeightMap) - (int) (x * MapToHeightMap) - 0.5f * MapToHeightMap;
                var yProp = (y * MapToHeightMap) - (int) (y * MapToHeightMap) - 0.5f * MapToHeightMap;
      
                HeightMap[x, y] = Map[tlx, tly].Elevation;
                if (xProp > 0.8f && Map[tlx + 1, tly].Elevation != Map[tlx, tly].Elevation)
                {
                    var threshold = BlendThreshold;
                    var x1 = threshold;
                    var x2 = 1 + (1 - threshold);
                    var h1 = Map[tlx, tly].Elevation;
                    var h2 = Map[tlx + 1, tly].Elevation;
                    var m = (h2 - h1) / (x2 - x1);
                    var n = -m * threshold;
                    var val = m * xProp + n;
                    HeightMap[x, y] += val;
                }
                if (yProp > 0.8f && Map[tlx, tly + 1].Elevation != Map[tlx, tly].Elevation)
                {
                    var threshold = BlendThreshold;
                    var x1 = threshold;
                    var x2 = 1 + (1 - threshold);
                    var h1 = Map[tlx, tly].Elevation;
                    var h2 = Map[tlx, tly + 1].Elevation;
                    var m = (h2 - h1) / (x2 - x1);
                    var n = -m * threshold;
                    var val = m * yProp + n;
                    HeightMap[x, y] += val;
                }
                if (yProp < 0.2f && Map[tlx, tly - 1].Elevation != Map[tlx, tly].Elevation)
                {
                    var threshold = 1 - BlendThreshold;
                    var x1 = -threshold;
                    var x2 = threshold;
                    var h1 = Map[tlx, tly - 1].Elevation;
                    var h2 = Map[tlx, tly].Elevation;
                    var m = (h2 - h1) / (x2 - x1);
                    var n = -m * threshold;
                    var val = m * yProp + n;
                    HeightMap[x, y] += val;
                }   
                if (xProp < 0.2f && Map[tlx - 1, tly].Elevation != Map[tlx, tly].Elevation)
                {
                    var threshold = 1 - BlendThreshold;
                    var x1 = -threshold;
                    var x2 = threshold;
                    var h1 = Map[tlx - 1, tly].Elevation;
                    var h2 = Map[tlx, tly].Elevation;
                    var m = (h2 - h1) / (x2 - x1);
                    var n = -m * threshold;
                    var val = m * xProp + n;
                    HeightMap[x, y] += val;
                }                  
                AlphaMaps[x, y, 0] = 1.0f;
            }            
        }
    }
    public void ApplyTerrainBrushes()
    {
        foreach (var mapTileKVP in  MapTiles)
        {
            var tile = mapTileKVP.Value;
            var HeightMapToTileRatioX = HeightMapWidth / Width;
            var HeightMapToTileRatioY = HeightMapHeight / Height;

            var brushStrokeCount = BrushStrokeCount;//Random.Range(0, 10);
            for (int i = 0; i < brushStrokeCount; i++)
            {
                var x = Random.Range(tile.TopLeft.x, tile.BottomRight.x) * HeightMapToTileRatioX;
                var y = Random.Range(tile.TopLeft.y, tile.BottomRight.y) * HeightMapToTileRatioY;
                //var size = (int)(Random.Range((tile.BottomRight.y - tile.TopLeft.y) * 0.5f, tile.BottomRight.y - tile.TopLeft.y)) * HeightMapToTileRatioX;
                var size = (int)(tile.BottomRight.y - tile.TopLeft.y) * HeightMapToTileRatioX; 

                BakeBrushOnHeightMap(tile,
                    new Vector2(x, y),
                    size,
                    0.0f);
            }
        }
    }

    private void BakeBrushOnHeightMap(MapTile tile, Vector2 pos, int size, float rotation)
    {
        if (tile.Terrain == "")
        {
            
        }
        var brush = Brushes[tile.Terrain][Random.Range(0, Brushes[tile.Terrain].Length - 1)];
        for (var x = pos.x;
            x < pos.x + size;
            x++)
        {
            for (var y = pos.y;
                y < pos.y + size;
                y++)
            {
                var xProp = (float) (x - pos.x) / size;
                var yProp = (float) (y - pos.y) / size;

                if (brush != null)
                {
                    int mapX = (int)x / (HeightMapWidth / Width);
                    int mapY = (int)y / (HeightMapHeight / Height);
                    int alphaX = (int)((float)x / ((float)HeightMapWidth / (float)AlphaMapResolution));
                    int alphaY = (int)((float)y / ((float)HeightMapHeight / (float)AlphaMapResolution));
                        
                    if (x > 0 && x < HeightMapWidth - 1 && y > 0 && y < HeightMapHeight - 1)
                    {
                        if (Map[mapX, mapY].Terrain == tile.Terrain)
                        {
                            var distX = (xProp - size / 2) / size;
                            var distY = (yProp - size / 2) / size;
                            var dist =  Mathf.Sqrt(distX * distX + distY * distY);

                            
                            
                            HeightMap[(int) x, (int) y] = Math.Max(HeightMap[(int) x, (int) y],
                                brush.GetPixelBilinear(xProp, yProp).r * 0.2f * (1.0f - dist));
                            
                            AlphaMaps[(int) alphaX, (int) alphaY, 0] = 1.0f;
                                /*if (tile.Terrain == "Mountain")
                                {
                                    AlphaMaps[(int) alphaX, (int) alphaY, 0] = 1.0f;
                                }
                                if (tile.Terrain == "Hills" || tile.Terrain == "Erosion")
                                {
                                    AlphaMaps[(int) alphaX, (int) alphaY, 2] = 1.0f;
                                }
                                if (tile.Terrain == "Plateau")
                                {
                                    AlphaMaps[(int) alphaX, (int) alphaY, 4] = 1.0f;
                                }
                                if (tile.Terrain == "Ridge" || tile.Terrain == "Canyon")
                                {
                                    AlphaMaps[(int) alphaX, (int) alphaY, 3] = 1.0f;
                                } */                               
                        }
                    }
                }
            }
        }
    }

    public void GenerateMap()
    {
        void SetSplat(string terrain, int id, float elevation)
        {
            var x = Random.Range(0, Width - 1);
            var y = Random.Range(0, Height - 1);

            MapTiles.Add(id, new MapTile()
            {
                Center = new Vector2(x,y),
                TopLeft =  new Vector2(x,y),
                BottomRight = new Vector2(x,y),
                Elevation =  elevation,
                Terrain = terrain,
                ID = id
            });
            Map[x, y] = (terrain, id, elevation);
            Grow(x, y, 1, MapTiles[id]);
        }

        Brushes = new Dictionary<string, Texture2D[]>()
        {
            {"Canyon", Resources.LoadAll("Terrain Assets/BrushTextures/Canyon", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()},
            {"Erosion", Resources.LoadAll("Terrain Assets/BrushTextures/Erosion", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()},
            {"Hills", Resources.LoadAll("Terrain Assets/BrushTextures/Hills", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()},
            {"Mountain", Resources.LoadAll("Terrain Assets/BrushTextures/Mountain", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()},
            {"Plateau", Resources.LoadAll("Terrain Assets/BrushTextures/Plateau", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()},
            {"Ridge", Resources.LoadAll("Terrain Assets/BrushTextures/Ridge", typeof(Texture2D)).Select(x=> x as Texture2D).ToArray()}
        };
        
        for (int i = 0; i < SplatAmount; i++)
        {
            int rnd = Random.Range(0, Brushes.Count - 1);
            var terrain = Brushes.ElementAt(rnd).Key;
            SetSplat(terrain, i, MinElevation);
        }
    }

    public void Grow(int x, int y, float growStrength, MapTile mapTile)
    {
        var distanceToCenterX = (mapTile.Center.x - x) * centerDistancePenalty;
        var distanceToCenterY = (mapTile.Center.y - y) * centerDistancePenalty;

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
        if (Map[targetX, targetY].Terrain != mapTile.Terrain)
        {
            Map[targetX, targetY] = (mapTile.Terrain, mapTile.ID, mapTile.Elevation);
            if (targetX < mapTile.TopLeft.x)
            {
                mapTile.TopLeft.x = targetX;
            }
            if (targetX > mapTile.BottomRight.x)
            {
                mapTile.BottomRight.x = targetX;
            }
            if (targetY < mapTile.TopLeft.y)
            {
                mapTile.TopLeft.y = targetY;
            }
            if (targetY > mapTile.BottomRight.y)
            {
                mapTile.BottomRight.y = targetY;
            }            
            if (Random.Range(0.0f, 1.0f) < growStrength)
            {
                for (int i = 0; i < GrowChildrenAmount; i++)
                {
                    /*if (letter.ToLower() == "d")
                    {
                        growStrength *= Math.Abs(Height - targetY) / (Height * 3);
                    }*/
                    Grow(targetX, targetY, growStrength * growStrenthFalloff, mapTile);
                }
            }
        }
    }
}
