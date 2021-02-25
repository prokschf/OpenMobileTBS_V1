using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMapTile : MonoBehaviour
{
    public List<TravelConnection> TravelConnections { get; set; } = new List<TravelConnection>();
    public GameRunner GameRunner { get; set; }
    public void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void OnMouseOver()
    {
        if (GameRunner == null || GameRunner.ActivePlayer == null || GameRunner.ActivePlayer.ActiveUnit == null)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            return;
        }
        if (GameRunner.ActivePlayer.ActiveUnit.CanWalkTo(this) == true)
        {
            transform.localScale = new Vector3(0.5f, 0.7f, 0.5f);            
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void OnMouseUpAsButton()
    {
        
            
    }
}