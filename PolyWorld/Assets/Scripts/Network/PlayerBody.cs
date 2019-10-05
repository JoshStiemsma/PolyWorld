using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyData{
	public BodyData(Vector3 _pos, Quaternion _rot,Vector3 _hPos, Quaternion _hRot,Vector3 _LHpos, Quaternion _LHrot,Vector3 _RHpos, Quaternion _RHrot){
		position = _pos;
		rotation = _rot;

		headPosition = _hPos;
		headRotation = _hRot;

		RightHandPosition = _RHpos;
		RightHandRotation = _RHrot;

		LeftHandPosition = _LHpos;
		LeftHandRotation = _LHrot;
	}
	public BodyData(Vector3 _pos, Quaternion _rot){
		position = _pos;
		rotation = _rot;
	}

	public Vector3 position;
    public Quaternion rotation;
    public Vector3 getBodyPos(){return this.position;}
    public Quaternion getBodyRot(){return this.rotation;}

    public Vector3 headPosition;
    public Quaternion headRotation;
 	public Vector3 getHeadPos(){return this.headPosition;}
    public Quaternion getHeadRot(){return this.headRotation;}

	public Vector3 RightHandPosition;
    public Quaternion RightHandRotation;
 	public Vector3 getRightPos(){return this.RightHandPosition;}
    public Quaternion getRightRot(){return this.RightHandRotation;}

 	public Vector3 LeftHandPosition;
    public Quaternion LeftHandRotation;
	public Vector3 getLeftPos(){return this.LeftHandPosition;}
    public Quaternion getLeftRot(){return this.LeftHandRotation;}


    public void UpdateVRData(Vector3 _pos, Quaternion _rot,Vector3 _hPos, Quaternion _hRot,Vector3 _LHpos, Quaternion _LHrot,Vector3 _RHpos, Quaternion _RHrot){
    	Debug.Log("New vr player data");
    	position = _pos;
		rotation = _rot;

		headPosition = _hPos;
		headRotation = _hRot;

		RightHandPosition = _RHpos;
		RightHandRotation = _RHrot;

		LeftHandPosition = _LHpos;
		LeftHandRotation = _LHrot;
    }
    public void UpdateFlatData(Vector3 _pos, Quaternion _rot){
    	position = _pos;
		rotation = _rot;
    }
}

[System.Serializable]
public class ServerPlayerObjects{
	public GameObject Head, Left, Right;
	
}
public struct State
{
    internal double timestamp;

    internal Vector3 bpos;
    internal Quaternion brot;

    internal Vector3 lpos;
    internal Quaternion lrot;

    internal Vector3 rpos;
    internal Quaternion rrot;

    internal Vector3 hpos;
    internal Quaternion hrot;


}
public class PlayerBody : MonoBehaviour {
	public double interpolationBackTime = 0.1;
    // We store twenty states with "playback" information
    State[] m_BufferedState = new State[20];
    // Keep track of what slots are used
    int m_TimestampCount;

    public LocalPlayerBody localBody;

	//public VRPlayerObjects VRBody;
	public ServerPlayerObjects Server_Body;


	public GameObject body;
	public NetworkPlayer Player;

	public GameObject netBody, VRObjects;
	//public GameObject PC_Cam;
	bool isLocal = false;
	public BodyData data;

	// public void SetIntroBody(int i){
	// 	if(i == 0){// vr
	// 		Rig.SetActive(true);
	// 		PC_Cam.SetActive(false);

	// 	}else{
	// 		PC_Cam.SetActive(true);
	// 		Rig.SetActive(false);
	// 	}
	// }

	public void SetLocalBody(LocalPlayerBody body){
       localBody = body;
    }
	void SetBodyObjects(int i){
		if(i == 0){// Local players dont need any server  visual stuff
			netBody.SetActive(false);
			VRObjects.SetActive(false);
		}else if(i == 1){// Local Non VR player dont need extra stuff just head
			VRObjects.SetActive(false);
		}
	}
	bool initiated = false;
	void Start () {
		if(Player == null)
			Player = this.transform.parent.GetComponentInChildren<NetworkPlayer>();
		isLocal = Player.getIsLocal();

		if(Player.getInitiated() == true)
			Initiate();
		
	}
	void Initiate(){
		if(isLocal){
			//if local we dont need any body parts
			//no vr body objects
			//no pc head
			//get local body
			// if(localBody == null)
			// 	localBody = Player.getLocalBody();

			SetBodyObjects(0);

			UpdateCurrentLocations(
				body,
				localBody.getVRPlayerObjects().Head, 
				localBody.getVRPlayerObjects().Left,
				localBody.getVRPlayerObjects().Right);
		}else{
			//if on the server 

			if(Player.getPlayertype() != PlayerType.VR)
			SetBodyObjects(1);

			UpdateCurrentLocations(
				body,
				Server_Body.Head, 
				Server_Body.Left,
				Server_Body.Right);
		}





		initiated=true;
	}

	void Update(){
		//if not started and player is ready to start
		if(!initiated && Player.getInitiated() == true)
			Initiate();
		if(!initiated) return;


		if(!isLocal){
			LerpMovement();
		}
		else{
			//send data outward, vrbody entered but if pc then theyre null and thats fine
			UpdateCurrentLocations(body,
				localBody.getVRPlayerObjects().Head,
				localBody.getVRPlayerObjects().Left,
				localBody.getVRPlayerObjects().Right);
		}
		
	}


//Create a new state
	public void NewState(State state){
		// Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
        for (int i = m_BufferedState.Length-1; i >= 1; i--)
        {
            m_BufferedState[i] = m_BufferedState[i-1];
        }
        m_BufferedState[0] = state;
        // Increment state count but never exceed buffer size
        m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);


        // Check integrity, lowest numbered state in the buffer is newest and so on
        for (int i = 0; i < m_TimestampCount-1; i++)
        {
            if (m_BufferedState[i].timestamp < m_BufferedState[i+1].timestamp)
            Debug.Log("State inconsistent");
        }


	}

	//places Server body objects where incoming data says
	void UpdateNetworkBody(){
		if(data ==null )return;

		body.transform.position = data.getBodyPos();
		body.transform.rotation = data.getBodyRot();

		if(Player.getPlayertype() == PlayerType.VR){
			Server_Body.Head.transform.position = data.getHeadPos();
			Server_Body.Head.transform.rotation = data.getHeadRot();

			Server_Body.Left.transform.position = data.getLeftPos();
			Server_Body.Left.transform.rotation = data.getLeftRot();

			Server_Body.Right.transform.position = data.getRightPos();
			Server_Body.Right.transform.rotation = data.getRightRot();
		}
	}


	//lerp between positions of incoming data
	void LerpMovement(){

		double currentTime = PhotonNetwork.time;
        double interpolationTime = currentTime - interpolationBackTime;
        // We have a window of interpolationBackTime where we basically play
        // By having interpolationBackTime the average ping, you will usually use interpolation.
        // And only if no more data arrives we will use extrapolation
 
        // Use interpolation
        // Check if latest state exceeds interpolation time, if this is the case then
        // it is too old and extrapolation should be used
        if (m_BufferedState[0].timestamp > interpolationTime)
        {
            for (int i = 0; i < m_TimestampCount; i++)
            {
                // Find the state which matches the interpolation time (time+0.1) or use last state
                if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount-1)
                {
                    // The state one slot newer (<100ms) than the best playback state
                    State rhs = m_BufferedState[Mathf.Max(i-1, 0)];
 
                    // The best playback state (closest to 100 ms old (default time))
                    State lhs = m_BufferedState[i];
 
                    // Use the time between the two slots to determine if interpolation is necessary
                    double length = rhs.timestamp - lhs.timestamp;
                    float t = 0.0F;
                    // As the time difference gets closer to 100 ms t gets closer to 1 in
                    // which case rhs is only used
                    if (length > 0.0001)
                        t = (float)((interpolationTime - lhs.timestamp) / length);
 
                    // if t=0 => lhs is used directly
                    // transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
                    // transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);


                    body.transform.position = Vector3.Lerp(lhs.bpos, rhs.bpos, t);
					body.transform.rotation = Quaternion.Slerp(lhs.brot, rhs.brot, t);

					

					if(Player.getPlayertype() == PlayerType.VR){
						Server_Body.Head.transform.position = Vector3.Lerp(lhs.hpos, rhs.hpos, t);
						Server_Body.Head.transform.rotation = Quaternion.Slerp(lhs.hrot, rhs.hrot, t);

						Server_Body.Left.transform.position = Vector3.Lerp(lhs.lpos, rhs.lpos, t);
						Server_Body.Left.transform.rotation = Quaternion.Slerp(lhs.lrot, rhs.lrot, t);

						Server_Body.Right.transform.position = Vector3.Lerp(lhs.rpos, rhs.rpos, t);
						Server_Body.Right.transform.rotation = Quaternion.Slerp(lhs.rrot, rhs.rrot, t);
					}
                    return;
                }
            }
        }
        else
        {
        	// Use extrapolation. Here we do something really simple and just repeat the last
        	// received state. You can do clever stuff with predicting what should happen.
            State latest = m_BufferedState[0];
            body.transform.position = latest.bpos;
			body.transform.rotation = latest.brot;

					

			if(Player.getPlayertype() == PlayerType.VR){
				Server_Body.Head.transform.position = latest.hpos;
				Server_Body.Head.transform.rotation = latest.hrot;


				Server_Body.Left.transform.position = latest.lpos;
				Server_Body.Left.transform.rotation = latest.lrot;

				Server_Body.Right.transform.position = latest.rpos;
				Server_Body.Right.transform.rotation = latest.rrot;
			}
        }
	}
	public BodyData getData(){
		return this.data;
	}
	

	//update body data in prep for sending to server
	public void UpdateCurrentLocations(GameObject Body, GameObject Head, GameObject Left, GameObject Right){
		if(Player.getPlayertype() == PlayerType.VR){
			data = new BodyData(
				Body.transform.position,
				Body.transform.rotation,

				Head.transform.position,
				Head.transform.rotation,

				Left.transform.position,
				Left.transform.rotation,

				Right.transform.position,
				Right.transform.rotation);
		}else{
			data = new BodyData(
				Body.transform.position,
				Body.transform.rotation);

		}

	}
	//on recieve new data, update data packets with new data
	public void InVRData(Vector3 _pos, Quaternion _rot,Vector3 _hPos, Quaternion _hRot,Vector3 _LHpos, Quaternion _LHrot,Vector3 _RHpos, Quaternion _RHrot){
		if(data!=null){
			data.UpdateVRData(
				_pos,
				_rot,

				_hPos,
				_hRot,

				_LHpos,
				_LHrot,

				_RHpos,
				_RHrot);
		}else{
			data = new BodyData(
				_pos,
				_rot,

				_hPos,
				_hRot,

				_LHpos,
				_LHrot,

				_RHpos,
				_RHrot);
		}
	}

	//on recieve new data, update data packets with new data
	public void InFlatData(Vector3 _pos, Quaternion _rot){
		if(data!=null){
		data.UpdateFlatData(
			_pos,
			_rot);
		}else{
			data = new BodyData(
				_pos,
				_rot);
		}
	}



}
