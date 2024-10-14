using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private Animation anim;
	public float OpenSpeed = 3;
	public float CloseSpeed = 3;
	public bool isAutomatic = false;
	public bool AutoClose = false;
	public bool DoubleSidesOpen = false;
	public string OpenForwardAnimName = "Door_anim";
	public string OpenBackwardAnimName = "DoorBack_anim";
	private string _animName;
	private bool inTrigger = false;
	[SerializeField] bool notOpen;
	private bool isOpen = false;
	private Vector3 relativePos;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		_animName = anim.clip.name;
	}
	void OpenDoor()
	{
		if (notOpen)
		{
			return;
		}
		anim [_animName].speed = 1 * OpenSpeed;
		anim [_animName].normalizedTime = 0;
		anim.Play (_animName);

	}
	void CloseDoor(){
		if (notOpen)
		{
			return;
		}
		anim [_animName].speed = -1 * CloseSpeed;
		if (anim [_animName].normalizedTime > 0) {
			anim [_animName].normalizedTime = anim [_animName].normalizedTime;
		} else {
			anim [_animName].normalizedTime = 1;
		}
		anim.Play (_animName);
	}

	void OnTriggerEnter(Collider other)
	{
		//if(other.GetComponent<Collider>().tag == PlayerHeadTag)
		//{

		//}
		if (notOpen)
        {
			return;
        }
		if (DoubleSidesOpen)
		{
			relativePos = gameObject.transform.InverseTransformPoint(other.transform.position);
			if (relativePos.z > 0)
			{
				_animName = OpenForwardAnimName;
			}
			else
			{
				_animName = OpenBackwardAnimName;
			}
		}
		if (isAutomatic)
		{
			OpenDoor();
		}

		inTrigger = true;
	}
	void OnTriggerExit(Collider other){
		if (notOpen)
        {
			return;
        }

		//if(other.GetComponent<Collider>().tag == PlayerHeadTag)
		//{
			
		//}
		if (isAutomatic)
		{
			CloseDoor();
		}
		else
		{
			inTrigger = false;
		}
		if (AutoClose && isOpen)
		{
			CloseDoor();
			inTrigger = false;
			isOpen = false;
		}
	}
}
