using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public abstract bool IsHuman { get; set; }
    public abstract void ProcessTurn(MainGameLoop MainGameLoop);
    public string Name { get; set; }
}