using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
	public CharacterController Controller;
	public Transform PathParent;
	private Transform[] Path;

	private const float Speed = 2.0f;

	private int CurrentPathNode;
	private Transform CurrentPathTransform => Path[CurrentPathNode];

    void Start()
    {
		Path = new Transform[PathParent.childCount];
		for(int i = 0; i < PathParent.childCount; i++)
		{
			Path[i] = PathParent.GetChild(i);
		}

		CurrentPathNode = 0;
	}

    void Update()
    {
		Vector3 direction = CurrentPathTransform.position - transform.position;
		direction.y = 0.0f;
		direction.Normalize();

		Controller.Move(direction * Speed * Time.deltaTime);
		transform.rotation = Quaternion.LookRotation(direction);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform == CurrentPathTransform)
		{
			CurrentPathNode = (CurrentPathNode + 1) % Path.Length;
		}
	}
}
