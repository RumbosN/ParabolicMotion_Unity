using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BattleGameManager : Singleton<BattleGameManager>
{
    public EPlayerId currentPlayer;

    [SerializeField] private Transform _worldTransform;
    [SerializeField] private AudioClip _newPointSound;
    [SerializeField] private AudioClip _pointAgainstSound;

    private Dictionary<EPlayerId, int> _playerPoints;
    private AudioSource _soundEffects;

    public Transform WorldTransform => _worldTransform;

    public Dictionary<EPlayerId, int> PlayerPoints => _playerPoints;

    void Start()
    {
        _playerPoints = new Dictionary<EPlayerId, int>();
        _soundEffects = GetComponent<AudioSource>();
        foreach (EPlayerId player in Enum.GetValues(typeof(EPlayerId)))
        {
            _playerPoints[player] = 0;
        }
    }

    public void IncreasePointsToPlayer(EPlayerId player)
    {
        _playerPoints[player] += 1;

        _soundEffects.clip = (currentPlayer == player) ? _newPointSound : _pointAgainstSound;
        _soundEffects.Play();

        Debug.Log($"Player 1 {_playerPoints[EPlayerId.PLAYER_1]} vs Player 2 {_playerPoints[EPlayerId.PLAYER_2]}");
    }
}
