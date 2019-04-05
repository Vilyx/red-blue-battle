using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner {

	public event System.Action<Character> characterSpawned;
	public event System.Action spawningFinished;

	[SerializeField]
	private GameObject blueCharPrefab;
	[SerializeField]
	private GameObject redCharPrefab;

	private float lastSpawn;
	private List<Character> characters = new List<Character>();

	public float SpawnDelay { get; set; }
	public int UnitsToSpawn { get; set; }
	public float MinUnitRadius { get; set; }
	public float MaxUnitRadius { get; set; }
	public float MinUnitSpeed { get; set; }
	public float MaxUnitSpeed { get; set; }
	public int GameAreaWidth { get; set; }
	public int GameAreaHeight { get; set; }

	private void Update () {
		if (UnitsToSpawn > 0)
		{
			if (Time.time - lastSpawn > SpawnDelay)
			{
				lastSpawn = Time.time;
				UnitsToSpawn--;
				Spawn();
			}
		}
		else
		{
			spawningFinished?.Invoke();

			gameObject.SetActive(false);
		}
	}


	/// <summary>
	/// Спавн одного персонажа
	/// </summary>
	/// <param name="team">За какую команду 0 - синие, 1 - красные, -1 - установить при спавне</param>
	/// <param name="size">Размер персонажа, если не установлено, то выберется случайно</param>
	/// <param name="angle">Поворот персонажа, если не установлено, то выберется случайно</param>
	/// <param name="speed">Начальная скорость, если не установлено, то выберется случайно</param>
	/// <param name="position">Положение, если не установлено, то выберется случайно в пределах игрового поля</param>
	public void Spawn(int team = -1, float size = -1, float angle = -1, float speed = -1, Vector3 position = default(Vector3))
	{
		if (team == -1)
			team = characters.Count % 2;
		GameObject currentPrefab = (team == 0 ? redCharPrefab : blueCharPrefab);
		GameObject go = Instantiate(currentPrefab);
		Character character = go.GetComponent<Character>();
		if (speed == -1)
			speed = Random.Range(MinUnitSpeed, MaxUnitSpeed);
		character.Speed = speed;
		if(size == -1)
			size = Random.Range(MinUnitRadius, MaxUnitRadius);
		character.Scale = size;
		if (angle == -1)
			angle = Random.value * 360;
		character.Angle = angle;

		if (object.Equals(position, default(Vector3)))
		{
			Vector3 randPos = new Vector3();
			int tries = 10000;
			do
			{
				tries--;
				float halfWidth = GameAreaWidth / 2 - size / 2;
				randPos.x = Random.Range(-halfWidth, halfWidth);
				float halfHeight = GameAreaHeight / 2 - size / 2;
				randPos.z = Random.Range(-halfHeight, halfHeight);
				character.Position = randPos;
			} while (IsPositionOccupied(character) && tries > 0);
		}
		else
		{
			character.Position = position;
		}

		characters.Add(character);
		characterSpawned?.Invoke(character);
	}


	/// <summary>
	/// Проверяет не пересекается ли новый персонаж с уже существующими
	/// </summary>
	/// <param name="candidateCharacter">Новый персонаж</param>
	/// <returns>true - пересекает, false - не пересекает</returns>
	private bool IsPositionOccupied(Character candidateCharacter)
	{
		for (int i = 0; i < characters.Count; i++)
		{
			if (Utils.IsColliding(candidateCharacter, characters[i]))
				return true;
		}
		return false;
	}
}
