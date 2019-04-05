using UnityEngine;

public class Utils {

	public static bool IsColliding(Character char1, Character char2)
	{
		float sqrCollisionDistance = Mathf.Pow(char1.Radius + char2.Radius, 2);
		float actualSquareDistance = (char1.Position - char2.Position).sqrMagnitude;
		return sqrCollisionDistance > actualSquareDistance;
	}

	public static bool IsCrossesGameArea(Character character, Bounds rect)
	{
		Vector3 position = character.Position;
		if (position.x - rect.min.x < character.Radius
			|| rect.max.x - position.x < character.Radius
			|| position.z - rect.min.z < character.Radius
			|| rect.max.z - position.z < character.Radius)
			return true;
		return false;
	}

	public static Vector3 Project(Vector3 lineS, Vector3 lineE, Vector3 toProject)
	{
		return NearestPointOnLine(lineS, lineS - lineE, toProject);
	}

	public static Vector3 NearestPointOnLine(Vector3 lineS, Vector3 lineDir, Vector3 toProject)
	{
		lineDir.Normalize();
		Vector3 v = toProject - lineS;
		float d = Vector3.Dot(v, lineDir);
		return lineS + lineDir * d;
	}
}
