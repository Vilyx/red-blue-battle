using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameConfig {

	public int gameAreaWidth;
	public int gameAreaHeight;
	public int unitSpawnDelay;
	public int numUnitsToSpawn;
	public float minUnitRadius;
	public float maxUnitRadius;
	public float minUnitSpeed;
	public float maxUnitSpeed;
}
