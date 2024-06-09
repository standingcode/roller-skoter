using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainGameManager : MonoBehaviour
{
	protected MainGameManager instance;
	public MainGameManager Instance { get => instance; set => instance = value; }

	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		Application.targetFrameRate = (int)Screen.mainWindowDisplayInfo.refreshRate.value;
		QualitySettings.vSyncCount = 1;
		//Application.targetFrameRate = 300;
	}

	public void OnQuitGame()
	{
		Application.Quit();
	}
}
