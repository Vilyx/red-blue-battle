using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public event Action<int, int> balanceChanged;

	[SerializeField]
	private GameObject spawnerPrefab;
	[SerializeField]
	private CameraPlacer cameraPlacer;
	[SerializeField]
	private SpriteRenderer gameAreaBackground;
	private List<Character> characters = new List<Character>();
	private ISpawner spawner;
	private GameConfig config;
	private Bounds gameAreaBounds;
	private string configFilename = "data.txt";
	private string saveFilename = "gamesave.txt";

	public float SimulationDuration { get; private set; }

	private void Start ()
	{
		RestartGame();
	}

	private void FixedUpdate()
	{
		CollisionSystem.CheckCollisions(characters, gameAreaBounds);
	}

	/// <summary>
	/// Полный перезапуск игры
	/// </summary>
	public void RestartGame()
	{
		WipeGame();
		LoadConfig();
		InitGameAreaBounds();
		InitSpawner();
		UpdateCameraPlacement();
	}


	/// <summary>
	/// Сохранение игры
	/// </summary>
	public void SaveGame()
	{
		GameSavePD gameSave = new GameSavePD();
		gameSave.simulationDuration = SimulationDuration;
		gameSave.spawner = new SpawnerDTO();
		gameSave.spawner.unitsToSpawn = spawner.UnitsToSpawn;

		gameSave.characters = new List<CharacterDTO>();
		for (int i = 0; i < characters.Count; i++)
		{
			Character character = characters[i];
			CharacterDTO characterDTO = new CharacterDTO();
			characterDTO.angle = character.Angle;
			characterDTO.position = character.Position;
			characterDTO.scale = character.Scale;
			characterDTO.team = character.Team;
			characterDTO.speed = character.Speed;
			gameSave.characters.Add(characterDTO);
		}
		gameSave.Save(saveFilename);
	}


	/// <summary>
	/// Загрузка игры
	/// </summary>
	public void LoadGame()
	{
		GameSavePD gameSave = new GameSavePD();
		gameSave.Load(saveFilename);
		RestartGame();
		List<CharacterDTO> characterDTOs = gameSave.characters;
		for (int i = 0; i < characterDTOs.Count; i++)
		{
			CharacterDTO dTO = characterDTOs[i];
			spawner.Spawn(dTO.team,dTO.scale,dTO.angle,dTO.speed,dTO.position);
		}
		spawner.UnitsToSpawn = gameSave.spawner.unitsToSpawn;
		SimulationDuration = gameSave.simulationDuration;
	}


	/// <summary>
	/// Стирает существующих персонажей и спавнера
	/// </summary>
	private void WipeGame()
	{
		SimulationDuration = 0;
		if (spawner != null)
			Destroy(spawner.gameObject);
		for (int i = 0; i < characters.Count; i++)
		{
			Destroy(characters[i].gameObject);
		}
		characters = new List<Character>();
	}


	/// <summary>
	/// Инициализирует gameAreaBounds данными из конфига и выставляет нужный размер фону
	/// </summary>
	private void InitGameAreaBounds()
	{
		gameAreaBounds = new Bounds();
		gameAreaBounds.size = new Vector3(config.gameAreaWidth, 0, config.gameAreaHeight);
		gameAreaBackground.size = new Vector2(config.gameAreaWidth, config.gameAreaHeight);
	}


	/// <summary>
	/// Загружает конфиг
	/// </summary>
	private void LoadConfig()
	{
		ConfigWrapperPD configWrapperPD = new ConfigWrapperPD();
		configWrapperPD.Load(configFilename);
		config = configWrapperPD.GameConfig;
	}


	/// <summary>
	/// Инициализация спавнера и подписка на его события
	/// </summary>
	private void InitSpawner()
	{
		spawner = Instantiate(spawnerPrefab).GetComponent<ISpawner>();
		spawner.GameAreaWidth = config.gameAreaWidth;
		spawner.GameAreaHeight = config.gameAreaHeight;
		spawner.MinUnitRadius = config.minUnitRadius;
		spawner.MaxUnitRadius = config.maxUnitRadius;
		spawner.MinUnitSpeed = config.minUnitSpeed;
		spawner.MaxUnitSpeed = config.maxUnitSpeed;
		spawner.UnitsToSpawn = config.numUnitsToSpawn;
		spawner.SpawnDelay = config.unitSpawnDelay / 1000.0f;

		spawner.characterSpawned += OnCharacterSpawn;
		spawner.spawningFinished += OnSpawnFinish;
	}


	/// <summary>
	/// Отдаляет камеру для охвата всего поля симуляции
	/// </summary>
	private void UpdateCameraPlacement()
	{
		Bounds bounds = new Bounds();
		bounds.size = new Vector3(config.gameAreaWidth, 0, config.gameAreaHeight);
		cameraPlacer.ScaleCamera(bounds);
	}


	/// <summary>
	/// Останавливает вновь созданного персонажа и подписывается на его смерть
	/// </summary>
	/// <param name="character">вновь созданный персонаж</param>
	private void OnCharacterSpawn(Character character)
	{
		character.Freezed = true;
		character.deadlyCollision += OnDeadlyCollision;
		characters.Add(character);
	}


	/// <summary>
	/// Все персонажи заспавнены, теперь надо разрешить им двигаться
	/// и обновить индикатор баланса сил
	/// </summary>
	private void OnSpawnFinish()
	{
		for (int i = 0; i < characters.Count; i++)
		{
			characters[i].Freezed = false;
		}
		UpdateWinrateUI();
	}


	/// <summary>
	/// Кто-то умер от столкновения
	/// </summary>
	/// <param name="character1">Один из столкнувшихся</param>
	/// <param name="character2">Второй столкнувшийся</param>
	private void OnDeadlyCollision(Character character1, Character character2)
	{
		if (character1 != null && characters.IndexOf(character1) != -1)
		{
			characters.Remove(character1);
			Destroy(character1.gameObject);
		}
		if (character2 != null && characters.IndexOf(character2) != -1)
		{
			characters.Remove(character2);
			Destroy(character2.gameObject);
		}
		UpdateWinrateUI();
	}


	/// <summary>
	/// Обновить индикатор баланса сил
	/// </summary>
	private void UpdateWinrateUI()
	{
		int redCount = 0;
		int blueCount = 0;
		for (int i = 0; i < characters.Count; i++)
		{
			if (characters[i].Team == 0)
				blueCount++;
			else
				redCount++;
		}
		balanceChanged?.Invoke(blueCount, redCount);
	}
}
