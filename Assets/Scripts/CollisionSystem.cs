using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem {

	public static void CheckCollisions(List<Character> characters, Bounds gameAreaBounds)
	{
		for (int i = 0; i < characters.Count; i++)
		{
			Character character1 = characters[i];
			for (int j = i + 1; j < characters.Count; j++)
			{
				Character character2 = characters[j];
				if (Utils.IsColliding(character1, character2))
				{
					Vector3 collisionPoint = character2.Position - character1.Position;
					collisionPoint = collisionPoint * character1.Scale / (character1.Scale + character2.Scale);
					collisionPoint = character1.Position + collisionPoint;
					character1.OnMyCollision(character2, collisionPoint);
					character2.OnMyCollision(character1, collisionPoint);
				}
			}
			if (Utils.IsCrossesGameArea(character1, gameAreaBounds))
			{
				Vector3 leftTop = gameAreaBounds.min;
				Utils.IsCrossesGameArea(character1, gameAreaBounds);
				Vector3 rightBottom = gameAreaBounds.max;
				Vector3 rightTop = new Vector3(rightBottom.x, 0, leftTop.z);
				Vector3 leftBottom = new Vector3(leftTop.x, 0, rightBottom.z);

				List<Vector3> projections = new List<Vector3>();
				projections.Add(Utils.Project(leftTop, rightTop, character1.Position));
				projections.Add(Utils.Project(rightTop, rightBottom, character1.Position));
				projections.Add(Utils.Project(rightBottom, leftBottom, character1.Position));
				projections.Add(Utils.Project(leftBottom, leftTop, character1.Position));

				Vector3 closest = projections[0];
				float closestSquare = float.MaxValue;
				for (int k = 0; k < projections.Count; k++)
				{
					float squareMagnitude = (projections[k] - character1.Position).sqrMagnitude;
					if (squareMagnitude < closestSquare)
					{
						closestSquare = squareMagnitude;
						closest = projections[k];
					}
				}
				character1.OnMyCollision(null, closest);
			}
		}
	}
}
