using UnityEngine;
using System.Collections;

public class GameEntitySpawner : MonoBehaviour
{
	public GameEntityType m_SpawnEntityType = GameEntityType.E_GameEntityNone;
	
	public GameEntityType GetSpawnType() { return m_SpawnEntityType; }

	public static GameEntitySpawner FindEntitySpawner( GameEntityType _EntityToSpawnType )
	{
		GameEntitySpawner matchingSpawner = null;
		
		Object[] spawners = FindObjectsOfType(typeof(GameEntitySpawner));
		foreach ( Object spawner in spawners )
		{
			GameEntitySpawner entitySpawner = spawner as GameEntitySpawner;
			GameEntityType spawnerType = entitySpawner.GetSpawnType();
			
			if (spawnerType == _EntityToSpawnType)
			{
				matchingSpawner = entitySpawner;
				break;
			}
		}
		
		return matchingSpawner;
	}
	
	void Start()
	{
		enabled = false;
	}
	
	void Update()
	{
	
	}
}
