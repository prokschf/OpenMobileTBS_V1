using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMapTile : MonoBehaviour
{
    public List<TravelConnection> TravelConnections { get; set; } = new List<TravelConnection>();
    public GameRunner GameRunner { get; set; }
    private Vector3 standardScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 raisedScale = new Vector3(0.1f, 0.2f, 0.1f);
    public void Start()
    {
        transform.localScale = standardScale;
    }

    public void OnMouseExit()
    {
        transform.localScale = standardScale;
    }

    public void OnMouseOver()
    {
        if (GameRunner == null || GameRunner.ActivePlayer == null || GameRunner.ActivePlayer.ActiveUnit == null)
        {
            transform.localScale = standardScale;
            return;
        }
        if (GameRunner.ActivePlayer.ActiveUnit.CanWalkTo(this) == true)
        {
            transform.localScale = raisedScale;            
        }
        else
        {
            transform.localScale = standardScale;
        }
    }

    public void OnMouseUpAsButton()
    {
        if (GameRunner == null || GameRunner.ActivePlayer == null || GameRunner.ActivePlayer.ActiveUnit == null)
        {
            return;
        }
        if (GameRunner.ActivePlayer.ActiveUnit.CanWalkTo(this) == true 
            && GameRunner.ActivePlayer.ActiveUnit.UnitActions.Any(x => x is MoveOnLandAction))
        {
            GameRunner.ActivePlayer.ActiveUnit.UnitActions.Single((x => x is MoveOnLandAction)).Perform(GameRunner.ActivePlayer.ActiveUnit, this);
            GameRunner.ActivePlayer.ActiveUnit = null;
        }        
    }
}