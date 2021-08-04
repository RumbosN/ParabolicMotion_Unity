using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PipeCounter : MonoBehaviourPun
{
    [SerializeField] private EPlayerId opposingPlayer;
    [SerializeField] private Material counterMaterial;

    private MeshRenderer _meshRenderer;
    private Material _initMaterial;

    void Start()
    {
        _meshRenderer = GetComponentInParent<MeshRenderer>();
        _initMaterial = _meshRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
	    IBulletController bulletController = other.gameObject.GetComponent<IBulletController>();

        if (bulletController != null)
        {
            bulletController.DontDespawn();
            _meshRenderer.material = counterMaterial;
            StartCoroutine(ResetMaterial());

            // Just increase the point when the currentUser is the owner of the SnowBall
            if (bulletController.IsMine())
            {
                IncreasePoint();
            }
        }
    }

    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.5f);
        _meshRenderer.material = _initMaterial;
    }

    private void IncreasePoint()
    { 
        object[] data = { opposingPlayer };
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte) PhotonEventCodes.INCREASE_POINT, data, options, SendOptions.SendReliable);
    }
}
