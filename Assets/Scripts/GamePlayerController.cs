using UnityEngine;
using System.Collections;

public class GamePlayerController : MonoBehaviour
{

	void Start()
	{
	
		DisablePlayerControllerUpdate();
	}
	
	void Update()
	{
	
	}
	
	public void EnablePlayerControllerUpdate()
	{
		enabled = true;	
	}
	
	public void DisablePlayerControllerUpdate()
	{
		enabled = false;
	}
}
