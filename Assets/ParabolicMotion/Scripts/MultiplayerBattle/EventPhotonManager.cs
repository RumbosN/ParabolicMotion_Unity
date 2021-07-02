using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EventPhotonManager : MonoBehaviour, IOnEventCallback
{
    private BattleGameManager _battleGameManager;
    
    void Start()
    {
        _battleGameManager = BattleGameManager.instance;
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        var eventCode = (PhotonEventCodes)photonEvent.Code;
        switch (eventCode)
        {
            case PhotonEventCodes.INCREASE_POINT:
                IncreasePoints((object[])photonEvent.CustomData);
                break;
        }
    }

    private void IncreasePoints(object[] data)
    {
        var playerId = (EPlayerId)data[0];
        _battleGameManager.IncreasePointsToPlayer(playerId);
    }
}
