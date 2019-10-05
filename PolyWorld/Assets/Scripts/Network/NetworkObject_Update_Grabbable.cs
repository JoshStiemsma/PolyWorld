using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkObject_Update_Grabbable : NetworkObject_Update {

 

    public Vector3 lastVelocity;
    public Vector3 lastAngularVelocity;

	public override void Awake(){
		base.Awake();
	}
	public override void Update(){
		base.Update();
	}
	public void FindVelocity(){
        if(lastPos!=null){
          lastVelocity =  (getRigidBody().transform.position - lastPos)/Time.fixedDeltaTime;
        }
        lastPos = getRigidBody().position;
    }
    public override void FixedUpdate(){
        base.FixedUpdate();
        if(view.isMine){
            FindVelocity();
        }
        
    }
    public override void OnReturnToScene(){
        Debug.Log("Set vel on scene obj");
        getRigidBody().velocity = lastVelocity;
        getRigidBody().angularVelocity = lastAngularVelocity;
    }

	public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
        	stream.SendNext(getRigidBody().transform.position);
            stream.SendNext(getRigidBody().transform.rotation);
           // Vector3 localangularvelocity = transform.InverseTransformDirection(RB.angularVelocity);
            stream.SendNext(lastVelocity);
            stream.SendNext(getRigidBody().angularVelocity);

        }else{
        	NetworkObjectState state = new NetworkObjectState();

          	Vector3  position = (Vector3)stream.ReceiveNext();
        	Quaternion  rotation = (Quaternion)stream.ReceiveNext();
            Vector3  vel = (Vector3)stream.ReceiveNext();
            Vector3  avel = (Vector3)stream.ReceiveNext();
        	
        	state.timestamp = info.timestamp;
        	state.pos = position;
        	state.rot = rotation;
            state.velocity = vel;
            state.AngularVelocity = avel;

        	NewState(state);
        }
    }



    public override void LerpMovement(){
        
        double currentTime = PhotonNetwork.time;
        double interpolationTime = currentTime - interpolationBackTime;
		NetworkObjectState[] m_BufferedState = getBufferState();
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
                    lastVelocity = Vector3.Lerp(lhs.velocity, rhs.velocity, t);
                    lastAngularVelocity = Vector3.Lerp(lhs.AngularVelocity, rhs.AngularVelocity, t);

                   getRigidBody().transform.position = lastPosition;
					getRigidBody().transform.rotation = lastRotation;

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
            lastVelocity = latest.velocity;
            lastAngularVelocity = latest.AngularVelocity;


            getRigidBody().transform.position = lastPosition;
			getRigidBody().transform.rotation = lastRotation;

			
        }
	}
}
