using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner {

	event System.Action<Character> characterSpawned;
	event System.Action spawningFinished;
	void Spawn(int team = -1, float size = -1, float angle = -1, float speed = -1, Vector3 position = default(Vector3));
	float SpawnDelay { get; set; }
	int UnitsToSpawn { get; set; }
	float MinUnitRadius { get; set; }
	float MaxUnitRadius { get; set; }
	float MinUnitSpeed { get; set; }
	float MaxUnitSpeed { get; set; }
	int GameAreaWidth { get; set; }
	int GameAreaHeight { get; set; }
	GameObject gameObject { get; }
}
