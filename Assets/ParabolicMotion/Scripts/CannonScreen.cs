using UnityEngine;
using UnityEngine.UI;

public class CannonScreen : MonoBehaviour
{
    [SerializeField] private Text _velocityText;
    [SerializeField] private Image _velocityBar;
    [SerializeField] private Text _angleText;
    [SerializeField] private Text _pointsP1;
    [SerializeField] private Text _pointsP2;

    private SnowWeapon _snowWeaponParent;
    private BattleGameManager _gameManager;
    
    void Start()
    {
        _snowWeaponParent = GetComponentInParent<SnowWeapon>();
        _gameManager = BattleGameManager.instance;
    }

    void Update()
    {
        UpdateVelocity();
        UpdateAngle();
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        var p1 = _gameManager.PlayerPoints[EPlayerId.PLAYER_1];
        var p2 = _gameManager.PlayerPoints[EPlayerId.PLAYER_2];
        _pointsP1.text = p1.ToString();
        _pointsP2.text = p2.ToString();
    }

    private void UpdateVelocity()
    {
        var velocity = _snowWeaponParent.CurrentVelocity;
        _velocityBar.fillAmount = velocity / _snowWeaponParent.maxVelocity;
        _velocityText.text = velocity.ToString("F2");
    }

    private void UpdateAngle()
    {
        var radianAngle = _snowWeaponParent.GetShootAngle();
        var degreeAngle = Mathf.CeilToInt(AngleUtils.RadiansToDegree(radianAngle));
        _angleText.text = $"{degreeAngle.ToString()}°";
    }
}
