using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
	public GameObject m_GameManagerPrefab = null;
	private LevelManager m_LevelManager = null;
	
	private bool m_IsActiveSequence = false;
	private bool IsSequenceActive() { return m_IsActiveSequence; }
	
	private int m_NextLevelIndex = -1;
	
	public string m_FirstLevelSceneFileName = "";
	public int m_FirstLevelSceneIndex = 0;
	
	public string m_CreditSceneFileName = "";
	public int m_CreditSceneIndex = 0;
	
	private int m_currentPanelIndex = 0;
	
	private bool m_enableUI = false;
	private bool IsUIEnable() { return m_enableUI; }
	private void EnableUI() { m_enableUI = true; }
	private void DisableUI() { m_enableUI = false; }
	
	void Start()
	{
		StartSequence();
		
		if (IsSequenceActive())
		{
			EnableUI();
		}
	}
	
	void OnLevelWasLoaded(int _level)
	{
	}
	
	void StartSequence()
	{
		LevelManager levelManager = (LevelManager)FindObjectOfType( typeof(LevelManager) );
		if (levelManager == null && m_GameManagerPrefab != null)
		{
			GameObject gameManager = Instantiate(m_GameManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			levelManager = gameManager.GetComponent<LevelManager>();
			
			Debug.Log("Instantiating gameManager!");
		}
		
		m_LevelManager = levelManager;
		
		m_IsActiveSequence = (m_LevelManager != null);

		InitCamera();
	}
	
	void EndSequence()
	{
		m_IsActiveSequence = false;
			
		m_LevelManager.LoadLevel(m_NextLevelIndex);
	}
	
	void InitCamera()
	{
		Camera mainCamera = Camera.main;
		mainCamera.orthographic = true;
	}
	
	private void QuitGame()
	{
		m_LevelManager.QuitGame();
	}
	
	private void RequestLevelTransition(int _LevelIndex)
	{
		m_NextLevelIndex = _LevelIndex;
	}
	
	private void UpdateLevelTransitionRequest(float _DeltaTime)
	{
		bool nextLevelIsValid = m_LevelManager.IsValidLevelIndex(m_NextLevelIndex);
		if (nextLevelIsValid)
		{
			EndSequence();
		}
	}
	
	private void UpdateMenuPanels(float _DeltaTime)
	{
		m_currentPanelIndex = 0;
		
		if (IsUIEnable())
		{
			m_currentPanelIndex = 1;
		}
	}
	
	void Update()
	{
		float deltaTime = Time.deltaTime;
		
		bool isSequenceActive = IsSequenceActive();
		if (isSequenceActive)
		{
			UpdateMenuPanels(deltaTime);
			
			UpdateLevelTransitionRequest(deltaTime);
		}
	}
	
	private void PlayGameButtonPressed()
	{
		RequestLevelTransition(m_FirstLevelSceneIndex);
		
		DisableUI();
	}
	
	private void AboutGameButtonPressed()
	{
		RequestLevelTransition(m_CreditSceneIndex);
		
		DisableUI();
	}
	
	private void QuitGameButtonPressed()
	{
		QuitGame();
		
		DisableUI();
	}

	void OnGUI ()
	{
		if (IsUIEnable())
		{
			//@FIXME: make all panel code data driven / editable from editor
			
			int panelSlideOffsetX = -200;
			
			int panelWidth = 200;
			int panelHeight = 400;
			
			int centerPanelOffsetX = Screen.width / 2 - panelWidth / 2;
			int centerPanelOffsetY = Screen.height / 2 - panelHeight / 2;
			
			int mainPanelIndex = 1;
			if (m_currentPanelIndex >= mainPanelIndex)
			{
				int mainPanelSlideCount = m_currentPanelIndex - mainPanelIndex;
				int mainPanelOffsetX = centerPanelOffsetX + mainPanelSlideCount * panelSlideOffsetX;
				int mainPanelOffsetY = centerPanelOffsetY;
				Rect mainPanel = new Rect( mainPanelOffsetX, mainPanelOffsetY, panelWidth, panelHeight);
				
				GUI.Box(mainPanel, "\n<b>Your Game</b>\n\nLet's Play!");
				
				int mainPanelButtonWidth = 160;
				int mainPanelButtonHeight = 40;
				int mainPanelButtonOffsetX = mainPanelOffsetX + (panelWidth - mainPanelButtonWidth) / 2;
				int mainPanelButtonOffsetY = mainPanelOffsetY + 160;
				int mainPanelButtonInterspaceY = 60;
				
				string createGameButtonLabel = "Play Game";
				if (GUI.Button(new Rect(mainPanelButtonOffsetX, mainPanelButtonOffsetY, mainPanelButtonWidth, mainPanelButtonHeight), createGameButtonLabel))
				{
					PlayGameButtonPressed();
				}
				mainPanelButtonOffsetY += mainPanelButtonInterspaceY;
				
				string aboutGameButtonLabel = "About";
				if (GUI.Button(new Rect(mainPanelButtonOffsetX, mainPanelButtonOffsetY, mainPanelButtonWidth, mainPanelButtonHeight), aboutGameButtonLabel))
				{
					AboutGameButtonPressed();
				}
				mainPanelButtonOffsetY += mainPanelButtonInterspaceY;
		
				string exitGameButtonLabel = "Exit";
				if (GUI.Button(new Rect(mainPanelButtonOffsetX, mainPanelButtonOffsetY, mainPanelButtonWidth, mainPanelButtonHeight), exitGameButtonLabel))
				{
					QuitGameButtonPressed();
				}
				mainPanelButtonOffsetY += mainPanelButtonInterspaceY;
			}
		}
	}
}
