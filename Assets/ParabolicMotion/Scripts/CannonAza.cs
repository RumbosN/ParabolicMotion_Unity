using UnityEngine;

public class CannonAza : MonoBehaviour
{
    [SerializeField] private GameObject _handModel;
    [SerializeField] private EHandSide _handSide;

    private Transform _transform;
    private float _minDistance;

    public EHandSide HandSide => _handSide;
    
    void Start()
    {
        InactiveHand();
        _transform = transform;
        _minDistance = BattleGameManager.instance.minDistanceHandToActiveAza;
    }

    public void ActiveHand()
    {
        _handModel.SetActive(true);
    }
    
    public void InactiveHand()
    {
        _handModel.SetActive(false);
    }

    public bool IsNear(Vector3 position)
    {
        var distance = (position - _transform.position).magnitude;
        return distance < _minDistance;
    }
}
