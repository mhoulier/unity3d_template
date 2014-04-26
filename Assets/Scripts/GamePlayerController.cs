using UnityEngine;
using System.Collections;

public class GamePlayerController : MonoBehaviour
{
	public float m_SpeedMultiplier = 1.0f;
	public float m_SpeedMinimum = 0.0f;

	public bool m_InvertLeftRightInput = false;

	float m_InputSpeed = 0.0f;
	float m_InputLeftRigthDirection = 0.0f;

	public float GetInputSpeed()
	{
		return m_InputSpeed;
	}

	public float GetInputLeftRightDirection()
	{
		return m_InputLeftRigthDirection;
	}
	
	void Start()
	{
	
		DisablePlayerControllerUpdate();
	}
	
	void Update()
	{
	
	}
	
	private void UpdatePlayerController()	
	{
		float input_h = Input.GetAxis("Horizontal");
		float input_v = Input.GetAxis("Vertical");

		//Debug.Log(string.Format("Inputs - h: {0}, v: {1}", input_h, input_v));

		m_InputSpeed = Mathf.Max( m_SpeedMinimum, m_SpeedMultiplier * (input_v*input_v) );
		m_InputLeftRigthDirection = (m_InvertLeftRightInput)? -input_h: input_h;
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
