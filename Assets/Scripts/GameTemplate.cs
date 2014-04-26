using UnityEngine;
using System.Collections;

public enum GameState { E_GameLoading, E_GameInitializing, E_GameWaitingForPlayers, E_GamePreparingForStart, E_GamePlaying, E_GamePaused, E_GameEnding, };

public enum GameEntityType { E_GameEntityNone, E_GamePlayer, E_GameOtherEntity, };

[System.Serializable]
public class GameEntitySpawnSettings
{
	public GameEntityType m_EntityType = GameEntityType.E_GameEntityNone;
	public GameObject m_EntityPrefab = null;
	
	public GameEntityType GetEntityType() { return m_EntityType; }
	public GameObject GetEntityPrefab() { return m_EntityPrefab; }
}

public class GameTemplate : MonoBehaviour
{
	public GameObject m_GameManagerPrefab = null;
	private LevelManager m_LevelManager = null;
	
	private bool m_IsActiveSequence = false;
	private bool IsSequenceActive() { return m_IsActiveSequence; }
	
	private int m_NextLevelIndex = -1;
	
	public GameEntitySpawnSettings[] m_EntitySpawnSettings;
	
	private GameState m_GameState = GameState.E_GameLoading;
	private void ChangeGameState(GameState _NewGameState) { m_GameState = _NewGameState; }
	
	private GameObject m_PlayerInstance = null;
	
	void OnLevelWasLoaded(int _level)
	{	
		LoadingFinished();
	}
	
	void Start()
	{
		StartSequence();
		
		WaitForPlayers();
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
		mainCamera.orthographic = false;
	}
	
	void Update()
	{
		float deltaTime = Time.deltaTime;
		
		if (m_IsActiveSequence)
		{
			switch (m_GameState)
			{
			case GameState.E_GameLoading:
				break;
			case GameState.E_GameWaitingForPlayers:
				UpdateGameWaitingForPlayers(deltaTime);
				break;
			case GameState.E_GamePreparingForStart:
				UpdateGamePreparingForStart(deltaTime);
				break;
			case GameState.E_GamePlaying:
				UpdateGamePlaying(deltaTime);
				break;
			case GameState.E_GamePaused:
				UpdateGamePaused(deltaTime);
				break;
			case GameState.E_GameEnding:
				UpdateGameEnding(deltaTime);
				break;
			}
		}
	}
	
	void LoadingFinished()
	{
		ChangeGameState(GameState.E_GameInitializing);
	}
	
	void WaitForPlayers()
	{
		ChangeGameState(GameState.E_GameWaitingForPlayers);
	}
	
	void PrepareForStart()
	{
		ChangeGameState(GameState.E_GamePreparingForStart);
		
		SpawnWaitingPlayers();
		
		//@TODO spawn AIs / NPCs / Pickups ...
	}
	
	private void SpawnWaitingPlayers()
	{
		GameEntityType playerType = GameEntityType.E_GamePlayer;
		GameEntitySpawner playerSpawner = GameEntitySpawner.FindEntitySpawner(playerType);
		if (playerSpawner != null)
		{
			GameEntitySpawnSettings spawnSettings = FindSpawnSettings(playerType);
			if (spawnSettings != null)
			{
				GameObject playerPrefab = spawnSettings.GetEntityPrefab();
				if (playerPrefab != null)
				{
					m_PlayerInstance = SpawnEntity(playerPrefab, playerSpawner.transform.position, playerSpawner.transform.rotation);
				}
			}
		}
	}
	
	private GameObject SpawnEntity(GameObject _EntityPrefab, Vector3 _Position, Quaternion _Rotation)
	{
		GameObject entityInstance = Instantiate(_EntityPrefab, _Position, _Rotation) as GameObject;
		
		return entityInstance;
	}
	
	private GameEntitySpawnSettings FindSpawnSettings( GameEntityType _EntityToSpawnType )
	{
		GameEntitySpawnSettings matchingSettings = null;
		
		foreach ( GameEntitySpawnSettings spawnSettings in m_EntitySpawnSettings )
		{
			GameEntityType spawnType = spawnSettings.GetEntityType();
			
			if (spawnType == _EntityToSpawnType)
			{
				matchingSettings = spawnSettings;
				break;
			}
		}
		
		return matchingSettings;
	}
	
	void StartGame()
	{
		ChangeGameState(GameState.E_GamePlaying);
		
		GameObject playerInstance = m_PlayerInstance;
		
		if (playerInstance != null)
		{
			GamePlayerController playerController = playerInstance.GetComponent<GamePlayerController>();
			System.Diagnostics.Debug.Assert(playerController != null);
			
			playerController.EnablePlayerControllerUpdate();
		}
		
		//@TODO - Enable NPC update
		//@TODO - Enable pickups, triggers, ... update
		
		UpdateCamera();
	}
	
	void UpdateGameWaitingForPlayers(float _DeltaTime)
	{
		PrepareForStart();
	}
	
	void UpdateGamePreparingForStart(float _DeltaTime)
	{
		StartGame();
	}
	
	void UpdateGamePlaying(float _DeltaTime)
	{
	}
	
	void UpdateGamePaused(float _DeltaTime)
	{
	}
	
	void UpdateGameEnding(float _DeltaTime)
	{
	}
	
	void UpdateCamera()
	{
		Camera mainCamera = Camera.main;
		GameCamera gameCamera = mainCamera.gameObject.GetComponent<GameCamera>();
		
		GameObject playerInstance = m_PlayerInstance;
		if (playerInstance != null)
		{
			gameCamera.SetFollowTarget(playerInstance.transform);
		}
	}
}
