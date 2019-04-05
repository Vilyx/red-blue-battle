﻿using UnityEngine;

public class Character : MonoBehaviour
{
	public System.Action<Character, Character> deadlyCollision;

	private float angle;
	private float scale;
	private Vector3 position;
	[SerializeField]
	private int team;
	private float deadlyRadius = 0.2f;

	public float Angle
	{
		get
		{
			return angle;
		}

		set
		{
			if (angle >= 360)
				angle = angle - 360;
			else if (angle < 0)
				angle = angle + 360;
			angle = value;
			transform.rotation = Quaternion.Euler(0, angle, 0);
		}
	}

	public float Scale
	{
		get
		{
			return scale;
		}

		set
		{
			scale = Mathf.Clamp(scale, 0, float.MaxValue);
			scale = value;
			Radius = scale / 2;
			transform.localScale = Vector3.one * scale;
		}
	}

	public Vector3 Position
	{
		get
		{
			return position;
		}

		set
		{
			position = value;
			transform.position = position;
		}
	}

	public int Team
	{
		get
		{
			return team;
		}
	}

	public float Radius { get; private set; }
	public bool Freezed { get; set; }
	public float Speed { get; set; }


	private void Update()
	{
		if (!Freezed)
		{
			Vector3 target = transform.position + transform.forward;
			float correctedSpeed = Speed * Time.deltaTime;
			Position = Vector3.MoveTowards(position, target, correctedSpeed);
		}
	}

	public void OnMyCollision(Character other, Vector3 collisionPoint)
	{
		if (other == null || other.Team == this.Team)
			Angle = GetAngle(collisionPoint, position);
		else
		{
			Scale = (position - collisionPoint).magnitude;
			if (scale < deadlyRadius * 2)
				deadlyCollision?.Invoke(this, other);
		}
	}

	private float GetAngle(Vector3 first, Vector3 second)
	{
		Vector3 from = first - second;
		Vector3 to = Vector3.Reflect(transform.forward, from.normalized);
		if (Vector3.Dot(from, transform.forward) > 0)
			return Quaternion.FromToRotation(Vector3.forward, to).eulerAngles.y;
		else
			return angle;
	}
}
