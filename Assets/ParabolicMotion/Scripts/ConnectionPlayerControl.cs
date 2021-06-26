using Photon.Pun;
using UnityEngine;

public class ConnectionPlayerControl : MonoBehaviourPunCallbacks
{ 
    private LogFeedback _logFeedback;
    private GameObject _spawnedPlayerPrefab;

    [SerializeField] private Transform ovrCameraRigTransform;
    [SerializeField] private Vector3 positionPlayer1;
    [SerializeField] private Vector3 rotationPlayer1;
    [SerializeField] private Vector3 positionPlayer2;
    [SerializeField] private Vector3 rotationPlayer2;
    [SerializeField] private GameObject snowBall;
    [SerializeField] private int snowBallAmount;

    private void Awake()
    {
        _logFeedback = LogFeedback.instance;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        _logFeedback.AddMessage("<Color=Green>OnJoinedRoom</Color> with " + playerCount + " Players(s)");

        Vector3 playerPosition = positionPlayer1 + 1.7f * Vector3.up;
        Quaternion playerRotation = Quaternion.Euler(rotationPlayer1);
        EPlayerId playerId = EPlayerId.PLAYER_1;

        if (playerCount % 2 == 0)
        { 
            playerPosition = positionPlayer2 + 1.7f * Vector3.up;
            playerRotation = Quaternion.Euler(rotationPlayer2);
            playerId = EPlayerId.PLAYER_2;
        }

        _spawnedPlayerPrefab = PhotonNetwork.Instantiate("PhotonAvatar", playerPosition, playerRotation, 0);
        ovrCameraRigTransform.position = playerPosition;
        ovrCameraRigTransform.rotation = playerRotation;

        UserPlayer player = _spawnedPlayerPrefab.GetComponent<UserPlayer>();
        player.SetupPlayer(playerId, ovrCameraRigTransform);
        BattleGameManager.instance.currentPlayer = playerId;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        _logFeedback.AddMessage("<Color=Red>OnLeftRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Players(s)");
        PhotonNetwork.Destroy(_spawnedPlayerPrefab);
    }
}
