using UnityEngine;

public class Utils {

	/// <summary>
	/// Пересекаются ли персонажи
	/// </summary>
	/// <param name="char1">Один</param>
	/// <param name="char2">Второй</param>
	/// <returns></returns>
	public static bool IsColliding(Character char1, Character char2)
	{
		float sqrCollisionDistance = Mathf.Pow(char1.Radius + char2.Radius, 2);
		float actualSquareDistance = (char1.Position - char2.Position).sqrMagnitude;
		return sqrCollisionDistance > actualSquareDistance;
	}


	/// <summary>
	/// Пересекает ли персонаж границу игровой зоны
	/// </summary>
	/// <param name="character">Персонаж</param>
	/// <param name="rect">Границы игровой зоны</param>
	/// <returns></returns>
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


	/// <summary>
	/// Перпендикулярная проекция точки на линию
	/// </summary>
	/// <param name="lineS">Первая точка линии</param>
	/// <param name="lineE">Вторая точка линии</param>
	/// <param name="toProject">Проецируемая точка</param>
	/// <returns></returns>
	public static Vector3 Project(Vector3 lineS, Vector3 lineE, Vector3 toProject)
	{
		return NearestPointOnLine(lineS, lineS - lineE, toProject);
	}

	/// <summary>
	/// Перпендикулярная проекция точки на линию
	/// </summary>
	/// <param name="lineS">Старт линии</param>
	/// <param name="lineE">Направление линии</param>
	/// <param name="toProject">Проецируемая точка</param>
	/// <returns></returns>
	public static Vector3 NearestPointOnLine(Vector3 lineS, Vector3 lineDir, Vector3 toProject)
	{
		lineDir.Normalize();
		Vector3 v = toProject - lineS;
		float d = Vector3.Dot(v, lineDir);
		return lineS + lineDir * d;
	}
}
