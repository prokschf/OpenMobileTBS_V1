using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int ID { get; set; }
    public UnitType UnitType { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int LastMovedOnTurn { get; set; }
    [SerializeField] private GameMapTile _mapTile;
    private Animator _animator;

    public GameMapTile MapTile
    {
        get => _mapTile;
        set
        {
            _mapTile = value;
            if (_animator != null)
            {
                _animator.SetBool("IsWalking", true);
            }
        }
    }

    public bool CanWalkTo(GameMapTile mapTileToCheck) => MapTile.TravelConnections.Any(x => x.Destination == mapTileToCheck);

    public void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Resources.Load("Toon_RTS_demo/ToonRTS_demo_Knight_AnimatorController") as RuntimeAnimatorController;
        _animator.SetBool("IsWalking", true);
    }

    public void Update()
    {
        if (MapTile == null)
        {
            Debug.Log("Unit {ID} has no assigned MapTile");
        }

        var targetPosition = MapTile.transform.position + Vector3.up * 0.25f;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.8f * Time.deltaTime);
        if (transform.position == targetPosition && _animator.GetBool("IsWalking"))
        {
            _animator.SetBool("IsWalking", false);
        }
    }
}