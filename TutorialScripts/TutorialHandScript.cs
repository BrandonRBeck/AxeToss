using UnityEngine;
using System.Collections;

public class TutorialHandScript : MonoBehaviour
{
	private bool move;
	private int shift;
	// Use this for initialization
	void Start ()
	{
		shift = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(move){
			if(transform.position.x < -5.79f){
				shift = 1;
			}else if(transform.position.x > -5.73f){
				shift = -1;
			}
			transform.position = new Vector3(transform.position.x + (.00143f * shift), transform.position.y, transform.position.z);
			transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + (.005f * shift), transform.rotation.w);
		}else{
			transform.position = new Vector3(-5.77f, transform.position.y, transform.position.z);
			transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
		}
	}

	public void setMove(bool move){
		this.move = move;
	}
}