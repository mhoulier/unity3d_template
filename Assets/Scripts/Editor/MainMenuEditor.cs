using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

[CustomEditor (typeof(MainMenu))]

public class MainMenuEditor : Editor
{
	private SerializedObject m_SerObj;
	private SerializedProperty m_GameManagerPrefabSerProp;
	private SerializedProperty m_PlayGameSceneNameSerProp;
	private SerializedProperty m_PlayGameSceneIndexSerProp;
	private SerializedProperty m_AboutSceneNameSerProp;
	private SerializedProperty m_AboutSceneIndexSerProp;

	public void OnEnable() 
	{
		m_SerObj = new SerializedObject(target);

		m_GameManagerPrefabSerProp = m_SerObj.FindProperty("m_GameManagerPrefab");
		m_PlayGameSceneNameSerProp = m_SerObj.FindProperty("m_FirstLevelSceneFileName");
		m_PlayGameSceneIndexSerProp = m_SerObj.FindProperty("m_FirstLevelSceneIndex");
		m_AboutSceneNameSerProp = m_SerObj.FindProperty("m_CreditSceneFileName");
		m_AboutSceneIndexSerProp = m_SerObj.FindProperty("m_CreditSceneIndex");
	}
	
	public override void OnInspectorGUI() 
	{
		//bool allowSceneObjects = !EditorUtility.IsPersistent(target);
		bool allowSceneObjectsForPrefab = false;

		m_SerObj.Update();
		
		
		EditorGUILayout.Separator();
		
		Object gameManagerPrefabObject = m_GameManagerPrefabSerProp.objectReferenceValue;
		gameManagerPrefabObject = EditorGUILayout.ObjectField("Game Manager Prefab", gameManagerPrefabObject, typeof(GameObject), allowSceneObjectsForPrefab);
		
		if (gameManagerPrefabObject != null)
		{
			m_GameManagerPrefabSerProp.objectReferenceValue = gameManagerPrefabObject;
		}
		
		EditorGUILayout.Separator();
		
		string playGameSceneName = m_PlayGameSceneNameSerProp.stringValue;
		string aboutSceneName = m_AboutSceneNameSerProp.stringValue;
		
		string[] sceneNameList = EditorBuildLevelList();
		int sceneCount = sceneNameList.Length;
		
		int playGameSceneIndex = 0;
		int aboutSceneIndex = 0;
		
		for (int sceneIndex = 0; sceneIndex < sceneCount; ++sceneIndex)
		{
			string sceneName = sceneNameList[sceneIndex];
			if (sceneName == playGameSceneName)
			{
				playGameSceneIndex = sceneIndex;
			}
			
			if (sceneName == aboutSceneName)
			{
				aboutSceneIndex = sceneIndex;
			}
		}
		
		playGameSceneIndex = EditorGUILayout.Popup("Play game scene", playGameSceneIndex, sceneNameList);
		aboutSceneIndex = EditorGUILayout.Popup("About scene", aboutSceneIndex, sceneNameList);
		
		m_PlayGameSceneIndexSerProp.intValue = playGameSceneIndex;
		m_PlayGameSceneNameSerProp.stringValue = sceneNameList[playGameSceneIndex];
		
		m_AboutSceneIndexSerProp.intValue = aboutSceneIndex;
		m_AboutSceneNameSerProp.stringValue = sceneNameList[aboutSceneIndex];

		m_SerObj.ApplyModifiedProperties();
	}
	
	private string[] EditorBuildLevelList()
	{
		EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
		int sceneCount = scenes.Length;
		
		string[] levels = new string[sceneCount];
		for (int sceneIndex = 0; sceneIndex < sceneCount; ++sceneIndex)
		{
			EditorBuildSettingsScene scene = scenes[sceneIndex];
			string scenePath = scene.path;
			string sceneFileName = Path.GetFileNameWithoutExtension(scenePath);
			levels[sceneIndex] = sceneFileName;
		}
		
		return levels;
	}
}
