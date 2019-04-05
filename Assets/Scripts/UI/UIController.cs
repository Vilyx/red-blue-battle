using UnityEngine;
using UnityEngine.UI;

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

	public void NewGame()
	{
		gameController.RestartGame();
	}

	public void SaveGame()
	{
		gameController.SaveGame();
	}

	public void LoadGame()
	{
		gameController.LoadGame();
	}

	public void SimulationSpeedChanged(float newValue)
	{
		Time.timeScale = newValue;
	}

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
