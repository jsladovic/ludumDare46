using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
	public Ability Ability;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}
}

public enum Ability
{
	None = 0,
	Electric = 1,
	Smelly = 2,
}
