using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

#pragma warning disable 649

public class ConnectionManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private byte maxPlayersPerRoom = 8;
    private LogFeedback _logFeedback;
    
    const string ROOM_NAME = "UTAD";
    bool isConnecting;

    string gameVersion = "1";
    bool launched = false;

    private void Awake()
    {
        _logFeedback = LogFeedback.instance; 
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!launched)
        {
            Connect();
            launched = true;
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
    }

    public void Connect()
    {
        // Texto en 3D que nos de informacion de lo que esta pasando
        _logFeedback.ReplaceMessage("");
        
        Debug.Log("Conectando .........");
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            _logFeedback.AddMessage("Joining Room...");
            PhotonNetwork.JoinRoom(ROOM_NAME);
        }
        else
        {
            _logFeedback.AddMessage("Connecting...");

            // Conectarse a los servidores de photon sin estar metido en una habitacion
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings(); // cuando es success llama a onConnectedToMaster
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            _logFeedback.AddMessage("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("OnConnectedToMaster...");
            
            PhotonNetwork.JoinRoom(ROOM_NAME);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _logFeedback.AddMessage("<Color=Red>OnJoinRoomFailed</Color>: Next -> Create a new room");
        CreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _logFeedback.AddMessage("<Color=Red>OnDisconnected</Color>" + cause);
        isConnecting = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnleftRoom");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _logFeedback.AddMessage("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Players(s)");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public void CreateRoom()
    {
        try
        {
            Debug.Log("Creating Room...");
            PhotonNetwork.CreateRoom(ROOM_NAME, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }
        catch (Exception e)
        {
            Debug.Log($"Error creating the Room: {e.Message}");
        }
    }
}
