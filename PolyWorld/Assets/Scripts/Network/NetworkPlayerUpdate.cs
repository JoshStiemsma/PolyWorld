using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using ExitGames;
public class NetworkPlayerUpdate : Photon.MonoBehaviour
{
    public NetworkPlayer NetPlayer;
    public PlayerBody Body;

    private Rigidbody RB;
    private float FORCEAMOUNT = 25;
    public bool isSending = true;

   //public GameObject PlayerBody;
   
    float LastFireTime = 0;
    float FireRate = .5f;
    public int chunkSize =90;
    private int onCount = 0;
    public Material TempMat;

    private bool setup = false;
    // Use this for initialization
    void Start()
    {
        NetPlayer = GetComponent<NetworkPlayer>();
        RB = GetComponent<Rigidbody>();
        //PlayerBodyToCopy.SetActive(false);
        Setup();
    }
    public void LoadChunk(int i){
        // SerializableMeshInfo SeriInfo = new SerializableMeshInfo(PlayerBodyToCopy,i,chunkSize);
        // lObj.AddData(SeriInfo,chunkSize,i);
    }
    public void LoadNextChunk(){
        // SerializableMeshInfo SeriInfo = new SerializableMeshInfo(PlayerBodyToCopy,onCount,chunkSize);
        // onCount++;
        // lObj.AddData(SeriInfo,chunkSize,onCount);

    }
    void Setup(){
        setup = true;

        ///SAVE THIS
        /*
   		// GameObject newBody = new GameObject("Loading Obj");
     //    lObj =  newBody.AddComponent<LoadeingObject>();
     //   lObj.cMesh =newBody.AddComponent<CombinedMesh>();

     //    lObj.SetMat(TempMat);

     //    newBody.AddComponent<MeshRenderer>();
     //    newBody.GetComponent<MeshRenderer>().material = TempMat;

     //    newBody.AddComponent<MeshFilter>();
     */
    }
  
    void Update()
    {

      
        

    }
    public bool SendingMesh = false;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        isSending = stream.isWriting;
        if (stream.isWriting)
        {

            stream.SendNext(Body.getData().getBodyPos());
            stream.SendNext(Body.getData().getBodyRot());
            if(this.NetPlayer.getPlayertype() == PlayerType.VR){
	            stream.SendNext(Body.getData().getHeadPos());
	            stream.SendNext(Body.getData().getHeadRot());

	            stream.SendNext(Body.getData().getLeftPos());
	            stream.SendNext(Body.getData().getLeftRot());

	            stream.SendNext(Body.getData().getRightPos());
	            stream.SendNext(Body.getData().getRightRot());
        	}
        


        }
        else
        {

          	Vector3  position = (Vector3)stream.ReceiveNext();
        	Quaternion  rotation = (Quaternion)stream.ReceiveNext();
        	State state = new State();
        	state.timestamp = info.timestamp;
        	state.bpos = position;
        	state.brot = rotation;

        
            if(this.NetPlayer.getPlayertype() == PlayerType.VR){

            	position = (Vector3)stream.ReceiveNext();
	        	rotation = (Quaternion)stream.ReceiveNext();
	        	state.hpos = position;
	        	state.hrot = rotation;

	        	position = (Vector3)stream.ReceiveNext();
	        	rotation = (Quaternion)stream.ReceiveNext();
	        	state.lpos = position;
	        	state.lrot = rotation;

	        	position = (Vector3)stream.ReceiveNext();
	        	rotation = (Quaternion)stream.ReceiveNext();
	        	state.rpos = position;
	        	state.rrot = rotation;





       		}else{

        		//Body.InFlatData(position,rotation);

       		}
        		Body.NewState(state);




            
        }
    }





















    
}