using UnityEngine;
using UnityEngine.UI;

public class CannonScreenBattle : CannonScreen
{
    [SerializeField] private Text _pointsP1;
    [SerializeField] private Text _pointsP2;

    private BattleGameManager _gameManager;

    protected override void Start()
    {
        base.Start();
        _gameManager = BattleGameManager.instance;
    }

    protected override void Update()
    {
        base.Update();
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        var p1 = _gameManager.PlayerPoints[EPlayerId.PLAYER_1];
        var p2 = _gameManager.PlayerPoints[EPlayerId.PLAYER_2];
        _pointsP1.text = p1.ToString();
        _pointsP2.text = p2.ToString();
    }
}
