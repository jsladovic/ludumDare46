using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public CharacterController Controller;
	private const float Speed = 2.0f;

    void Start()
    {
        
    }

    void Update()
	{
		Vector3 direction = Vector3.zero;
		if (Input.GetAxisRaw("Horizontal") != 0.0f)
		{
			direction.x = Input.GetAxisRaw("Horizontal");
		}
		if (Input.GetAxisRaw("Vertical") != 0.0f)
		{
			direction.z = Input.GetAxisRaw("Vertical");
		}
		if (direction != Vector3.zero)
		{
			direction.Normalize();
			transform.rotation = Quaternion.LookRotation(direction);
			Controller.Move(direction * Speed * Time.deltaTime);
		}
	}
}
