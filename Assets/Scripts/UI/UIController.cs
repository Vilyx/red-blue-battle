using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за взаимодействие интерфейса с симуляцией
/// </summary>
public class UIController : MonoBehaviour {

	[SerializeField]
	private GameController gameController;
	[SerializeField]
	private WinrateUI winrateUI;
	[SerializeField]
	private GameObject endgamePanel;
	[SerializeField]
	private Text durationText;
	[SerializeField]
	private WinrateUI endgameWinner;

	private void Awake()
	{
		gameController.balanceChanged += OnBalanceChanged;
	}


	/// <summary>
	/// Новая игра
	/// </summary>
	public void NewGame()
	{
		gameController.RestartGame();
	}


	/// <summary>
	/// Сохранение
	/// </summary>
	public void SaveGame()
	{
		gameController.SaveGame();
	}


	/// <summary>
	/// Загрузка
	/// </summary>
	public void LoadGame()
	{
		gameController.LoadGame();
	}


	/// <summary>
	/// Изменение скорости симуляции
	/// </summary>
	/// <param name="newValue">новое значение скорости воспроизведения, 
	/// 1 - нормальная скорость</param>
	public void SimulationSpeedChanged(float newValue)
	{
		Time.timeScale = newValue;
	}


	/// <summary>
	/// Реакция на изменение числа персонажей
	/// </summary>
	/// <param name="blueCount">число синих персонажей</param>
	/// <param name="redCount">число красных персонажей</param>
	private void OnBalanceChanged(int blueCount, int redCount)
	{
		winrateUI.SetWinrate(blueCount, redCount);
		if (blueCount == 0 || redCount == 0)
		{
			endgamePanel.SetActive(true);
			durationText.text = gameController.SimulationDuration.ToString();
			endgameWinner.SetWinrate(blueCount, redCount);
		}
	}
}
