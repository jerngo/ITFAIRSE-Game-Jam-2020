using UnityEngine;

public class PointInTime
{

	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public float axisInput;

	public PointInTime(Vector3 _position, Quaternion _rotation, Vector3 _localscale, float _axisImput)
	{
		position = _position;
		rotation = _rotation;
		scale = _localscale;
		axisInput = _axisImput;
	}

}
