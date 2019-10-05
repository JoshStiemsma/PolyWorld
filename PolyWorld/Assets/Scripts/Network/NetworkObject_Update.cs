using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct NetworkObjectState
{
    internal double timestamp;

    internal Vector3 pos;
    internal Quaternion rot;

    internal Vector3 velocity;
    internal Vector3 AngularVelocity;

    public Vector3 GetPosition(){return this.pos;}
    public Quaternion GetRotation(){return this.rot;}
    public Vector3 GetVelocity(){return this.velocity;}
    public Vector3 GetAngularVelocity(){return this.AngularVelocity;}


}

public class NetworkObject_Update : MonoBehaviour {


	public double interpolationBackTime = 0.1;
    // We store twenty states with "playback" information
     public NetworkObjectState[] getBufferState(){return this.m_BufferedState;}
    NetworkObjectState[] m_BufferedState = new NetworkObjectState[20];
    // Keep track of what slots are used
    public int m_TimestampCount;

	public Rigidbody RB;
    public Rigidbody getRigidBody(){return this.RB;}
	public PhotonView view;

    public Vector3 lastPosition;
    public Quaternion lastRotation;


	public virtual void Awake(){
        if(RB==null)
		RB =this.GetComponent<Rigidbody>();
		view= this.GetComponent<PhotonView>();
	}
	public virtual void Update(){
		if(!view.isMine){
           LerpMovement();
		}
	}
   public Vector3 lastPos ;//= Vector3.zero;
    public virtual void FixedUpdate(){
        if(view.isMine){
        }
    }
    
    public virtual void OnReturnToScene(){
    }
    public void NewState(NetworkObjectState state){
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

	public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
        	stream.SendNext(RB.transform.position);
            stream.SendNext(RB.transform.rotation);

        }else{
        	NetworkObjectState state = new NetworkObjectState();

          	Vector3  position = (Vector3)stream.ReceiveNext();
        	Quaternion  rotation = (Quaternion)stream.ReceiveNext();
        	
        	state.timestamp = info.timestamp;
        	state.pos = position;
        	state.rot = rotation;
        	NewState(state);
        }
    }



    public virtual void LerpMovement(){

        double currentTime = PhotonNetwork.time;
        double interpolationTime = currentTime - interpolationBackTime;

        if (m_BufferedState[0].timestamp > interpolationTime)
        {
            for (int i = 0; i < m_TimestampCount; i++)
            {
                // Find the state which matches the interpolation time (time+0.1) or use last state
                if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount-1)
                {
                    // The state one slot newer (<100ms) than the best playback state
                    NetworkObjectState rhs = m_BufferedState[Mathf.Max(i-1, 0)];
 
                    // The best playback state (closest to 100 ms old (default time))
                    NetworkObjectState lhs = m_BufferedState[i];
 
                    // Use the time between the two slots to determine if interpolation is necessary
                    double length = rhs.timestamp - lhs.timestamp;
                    float t = 0.0F;
                    // As the time difference gets closer to 100 ms t gets closer to 1 in
                    // which case rhs is only used
                    if (length > 0.0001)
                        t = (float)((interpolationTime - lhs.timestamp) / length);
                    lastPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
                    lastRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                  
                    RB.transform.position = lastPosition;
					RB.transform.rotation = lastRotation;

                    return;
                }
            }
        }
        else
        {
        	// Use extrapolation. Here we do something really simple and just repeat the last
        	// received state. You can do clever stuff with predicting what should happen.
            NetworkObjectState latest = m_BufferedState[0];

            lastPosition = latest.pos;
            lastRotation = latest.rot;

            RB.transform.position = lastPosition;
			RB.transform.rotation = lastRotation;

			
        }
	}
}
