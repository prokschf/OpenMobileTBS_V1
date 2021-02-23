using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType UnitType { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int LastMovedOnTurn { get; set; }
}