using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	private int m_NetworkLastLevelPrefix = 0;
	
	private bool m_LoadingLevel = true;
	
	public bool IsLoading() { return m_LoadingLevel; }
	
	public int GetCurrentLevelIndex() { return Application.loadedLevel; }
	public int GetLevelCount() { return Application.levelCount; }
	
	public bool IsValidLevelIndex(int _levelIndex)
	{
		int levelCount = GetLevelCount();
		bool IsValidIndex = (0 <= _levelIndex) && (_levelIndex < levelCount);
		return IsValidIndex;
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		if (networkView != null)
		{
			networkView.group = 1;
		}
	}
	
	void Start()
	{
		
	}

	void OnLevelWasLoaded(int _Level)
	{
		m_LoadingLevel = false;

		Debug.Log( string.Format("Loaded level {0} ...", _Level) );
	}

	public void LoadLevel(int _Level)
	{
		int levelCount = GetLevelCount();
		int levelToLoad = _Level % levelCount;

		Debug.Log( string.Format("Loading level {0} ...", levelToLoad) );

		m_LoadingLevel = true;

		Application.LoadLevel(levelToLoad);
	}
	
	public void LoadNextLevel()
	{
		int currentLevelIndex = GetCurrentLevelIndex();
		int levelCount = GetLevelCount();
		int nextLevelIndex = (currentLevelIndex + 1) % levelCount;
		
		LoadLevel(nextLevelIndex);
	}

	public IEnumerator LoadLevelAsync(int _Level, float _WaitTimeInSeconds)
	{
		yield return new WaitForSeconds(_WaitTimeInSeconds);

		LoadLevel(_Level);
	}

	public IEnumerator LoadNextLevelAsync(float _WaitTimeInSeconds)
	{
		yield return new WaitForSeconds(_WaitTimeInSeconds);

		LoadNextLevel();
	}
	
	public void ServerLoadLevel(int _Level)
	{
		System.Diagnostics.Debug.Assert(Network.isServer);
		
		Network.RemoveRPCsInGroup(0);
		Network.RemoveRPCsInGroup(1);
		
		//@NOTE: not using RPC mode All Buffered so that client doesn't load automatically as soon as it connected the server
		RPCMode loadLevelRPCMode = RPCMode.All;
		networkView.RPC("LoadLevelRPC", loadLevelRPCMode, _Level, m_NetworkLastLevelPrefix + 1);
	}
	
	public void ClientLoadLevelRequest(NetworkPlayer _LocalNetClient)
	{
		System.Diagnostics.Debug.Assert(Network.isClient);
		
		networkView.RPC("ServerLoadLevelRequestRPC", RPCMode.Server, _LocalNetClient);
	}
	
	[RPC]
	public void ServerLoadLevelRequestRPC(NetworkPlayer _NetClient)
	{
		System.Diagnostics.Debug.Assert(Network.isServer);
		
		int currentLevelIndex = GetCurrentLevelIndex();
		networkView.RPC( "LoadLevelRPC", _NetClient, currentLevelIndex, m_NetworkLastLevelPrefix);
	}
	
	[RPC]
	public void LoadLevelRPC( int _Level, int _LevelPrefix )
	{
		StartCoroutine(NetworkLoadLevelAsync(_Level, _LevelPrefix));
	}
	
	public IEnumerator NetworkLoadLevelAsync(int _Level, int _LevelPrefix)
	{
		m_NetworkLastLevelPrefix = _LevelPrefix;
		
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);	

		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;

		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		Network.SetLevelPrefix(_LevelPrefix);
		
		yield return StartCoroutine(LoadLevelAsync(_Level, 1.0f));
		//yield return new WaitForEndOfFrame();
		//LoadLevel(_level);

		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);

		//for (var go in FindObjectsOfType(GameObject))
		//	go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}

	void Update()
	{
	}
}
