using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public GameRunner GameRunner { get; set; }
    public abstract bool IsHuman { get; set; }
    public abstract void ProcessTurn();
    public string Name { get; set; }
    public List<Unit> Units { get; set; } = new List<Unit>();
    public List<City> Cities { get; set; } = new List<City>();

    private Unit _activeUnit;
    public Unit ActiveUnit
    {
        get
        {
            if (_activeUnit == null)
            {
                _activeUnit = Units.FirstOrDefault(x => x.LastMovedOnTurn < GameRunner.TurnCounter);
            }
            return _activeUnit;
        }
        private set
        {
            _activeUnit = value;
        }
    }

    public Dictionary<RessourceType, int> Ressources { get; set; } = new Dictionary<RessourceType, int>();
}