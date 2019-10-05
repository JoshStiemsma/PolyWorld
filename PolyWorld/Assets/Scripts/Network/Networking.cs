using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Networking : MonoBehaviour {

    public int DataCount = 0;
    public int currentIndex = 0;

    public int LastDataRecived = 0;


    private float test = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if Sever
        if (stream.isWriting)
        {
          
        }
        else
        {
            //Get updated client on data
            this.LastDataRecived = (int)stream.ReceiveNext();

        }

        //if client
        if (stream.isWriting)
        {
            //update server with data count
            stream.SendNext((int)LastDataRecived);
        }
        else
        {
            int msg = (int)stream.ReceiveNext();
            if (msg == 0)
                return;
            DataCount = (int)stream.ReceiveNext();
           
            //inData.Add(data);
        }



    }
    //send rpc to master asking to restert sending data
    
}
