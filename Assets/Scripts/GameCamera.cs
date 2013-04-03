using UnityEngine;
using System.Collections;

public abstract class GameCamera : MonoBehaviour
{
	public abstract void SetFollowTarget(Transform _FollowTarget);
}
