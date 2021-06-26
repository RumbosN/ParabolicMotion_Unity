using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class PhotonAvatarView : MonoBehaviour,IPunObservable
{
    public  OvrAvatar             avatar;
    private int                   localSequence;
    private PhotonView            photonView;
    private OvrAvatarRemoteDriver remoteDriver;
    private List<byte[]>          packetData;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            OvrAvatarDriver driver = avatar.gameObject.AddComponent<OvrAvatarLocalDriver>();
            
            avatar.Driver = driver;
            avatar.ShowThirdPerson = false;
            avatar.ShowFirstPerson = true;
            packetData = new List<byte[]>();
        }
        else
        {
            remoteDriver = avatar.gameObject.AddComponent<OvrAvatarRemoteDriver>();
            avatar.Driver = remoteDriver;            
        }
        avatar.gameObject.SetActive(true); 
    }

    public void OnEnable()
    {
        if (photonView.IsMine)
        {
            avatar.RecordPackets = true;
            avatar.PacketRecorded += OnLocalAvatarPacketRecorded;
        }
    }

    public void OnDisable()
    {
        if (photonView.IsMine)
        {
            avatar.RecordPackets = false;
            avatar.PacketRecorded -= OnLocalAvatarPacketRecorded;
        }
    }    

    public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        using (MemoryStream outputStream = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(outputStream);

            var size = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
            byte[] data = new byte[size];
            Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

            writer.Write(localSequence++);
            writer.Write(size);
            writer.Write(data);

            packetData.Add(outputStream.ToArray());
        }
    }

    private void DeserializeAndQueuePacketData(byte[] data)
    {
        
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int remoteSequence = reader.ReadInt32();

            int size = reader.ReadInt32();
            byte[] sdkData = reader.ReadBytes(size);

            System.IntPtr packet = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((System.UInt32)data.Length, sdkData);
            remoteDriver.QueuePacket(remoteSequence, new OvrAvatarPacket { ovrNativePacket = packet });
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
 
        if (stream.IsWriting)
        {
            if (packetData == null) 
            {
                return;
            }

            stream.SendNext(packetData.Count);

            foreach (byte[] b in packetData)
            {
                stream.SendNext(b);
            }

            packetData.Clear();
        }

        if (stream.IsReading)
        {
            int num = (int)stream.ReceiveNext();

            for (int counter = 0; counter < num; ++counter)
            {
                byte[] data = (byte[])stream.ReceiveNext();

                DeserializeAndQueuePacketData(data);
            }
        }
    }

}