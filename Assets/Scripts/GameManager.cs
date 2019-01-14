/**
 * Created by: Victoria Shenkevich
 * Created on: 12/01/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	#region Constants
	#endregion


	#region Private Fields
	private int experience = 0;
    #endregion


	#region Public Fields
    #endregion


	#region Prefabs
	#endregion


	#region Singleton
    private static GameManager instance = null;

    internal static GameManager Instance 
	{
		get 
		{ 
			if (instance == null)
				instance = (GameManager) FindObjectOfType(typeof(GameManager));		

			if (instance == null)
				instance = new GameObject ("GameManager").AddComponent<GameManager>();
				
			return instance;
		}
    }
    #endregion Singleton


	#region Unity Methods
	void Awake () 
	{
		//Singleton
		if (instance)
			DestroyImmediate(gameObject);
		else
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		//		
	}

	void Start () 
	{
		// DataManager.Instance.SetStorageType(DataManager.StorageType.PlayerPrefs);
		DataManager.Instance.SetStorageType(DataManager.StorageType.File);
		
		DataManager.Instance.SaveParam("username", "Victoria");
		DataManager.Instance.SaveParam("score", 30);
		DataManager.Instance.SaveParam("winPercentage", 0.75f);
		DataManager.Instance.SaveParam("isUserAuthorized", true);
		experience = DataManager.Instance.GetParamInt ("experience");

		Debug.Log(DataManager.Instance.GetParamString ("username"));
		Debug.Log(DataManager.Instance.GetParamInt ("score"));
		Debug.Log(DataManager.Instance.GetParamFloat ("winPercentage"));
		Debug.Log(DataManager.Instance.GetParamBool ("isUserAuthorized"));
		Debug.Log(experience);
	}
	
	void Update () 
	{
		experience++;
		DataManager.Instance.SaveParam("experience", experience);
	}
	#endregion


	#region Private Methods
    #endregion
	

	#region Public Methods
    #endregion
}