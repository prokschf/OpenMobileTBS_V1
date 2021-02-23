using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public abstract bool IsHuman { get; set; }
    public abstract void ProcessTurn(MainGameLoop MainGameLoop);
    public string Name { get; set; }
    public List<Unit> Units { get; set; }
    public List<City> Cities { get; set; }
    
    public Dictionary<RessourceType, int> Ressources { get; set; }
}