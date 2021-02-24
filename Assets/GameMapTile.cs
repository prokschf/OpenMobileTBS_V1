using System.Collections.Generic;
using UnityEngine;

public class GameMapTile : MonoBehaviour
{
    public List<TravelConnection> TravelConnections { get; set; } = new List<TravelConnection>();

    public void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}