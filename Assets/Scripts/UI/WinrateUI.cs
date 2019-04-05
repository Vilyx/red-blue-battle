using UnityEngine;
using UnityEngine.UI;

public class WinrateUI : MonoBehaviour {

	[SerializeField]
	private LayoutElement blue;
	[SerializeField]
	private LayoutElement red;

	public void SetWinrate(int blueCount, int redCount)
	{
		blue.flexibleWidth = blueCount;
		red.flexibleWidth = redCount;
	}
}
