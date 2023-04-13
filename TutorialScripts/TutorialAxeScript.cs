using UnityEngine;
using System.Collections;

public class TutorialAxeScript : MonoBehaviour
{
	private bool spin;
	private bool throwAxe;
	private bool rotateAround;
	private float spinSpeed;
	private Rigidbody2D rigidBody;
	private HingeJoint2D joint;
	private Transform transform;
	private bool wasThrown;
	private float force;

	public Transform target;

	// Use this for initialization
	void Start ()
	{
		force = 800f;
		rotateAround = true;
		spinSpeed = 4;
		rigidBody = GetComponent<Rigidbody2D>();
		transform = GetComponent<Transform>();
		joint = GetComponent<HingeJoint2D>();
		rigidBody.gravityScale = 0;
		rigidBody.velocity = new Vector2(0,0);
		rigidBody.freezeRotation = true;
		rigidBody.inertia = 0;
		rigidBody.isKinematic = true;
		wasThrown = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(spin){
			if(rotateAround){
				transform.RotateAround(target.position, new Vector3(0, 0, -1), spinSpeed); 
			}else{
				transform.Rotate(new Vector3(0,0,-20));
			}
		}
		if(throwAxe && !wasThrown){
			Destroy(joint);
			wasThrown = true;
			rigidBody.isKinematic = false;
			rotateAround = false;

			float angle = transform.rotation.eulerAngles.z - 30;

			float x =  Mathf.Sin(angle * Mathf.Deg2Rad);
			float y =  Mathf.Cos(angle * Mathf.Deg2Rad);

			Debug.Log("force" + force);
			Debug.Log("x" + x);
			Debug.Log("y" + y);
			rigidBody.AddForce(new Vector2(x * force * -1,y * force));
			rigidBody.gravityScale = 1;
		}

	}
	public void setSpin(bool spin){
		this.spin = spin;
	}
	public void setThrowAxe(bool throwAxe){
		this.throwAxe = throwAxe;
	}

}

