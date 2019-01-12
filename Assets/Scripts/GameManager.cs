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
			{
				GameObject go = GameObject.Find ("GameManager");

				if (go != null)
				{
					instance = go.GetComponent<GameManager>();
				}
			}				

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
		DataManager.Instance.SaveParam("username", "Victoria");
		DataManager.Instance.SaveParam("level", 3);
		DataManager.Instance.SaveParam("experience", 150);
		DataManager.Instance.SaveParam("health", 0.75f);
		DataManager.Instance.SaveParam("score", 500);	
		DataManager.Instance.SaveParam("firstSession", true);		

		Debug.Log(DataManager.Instance.GetParamString ("username"));
		Debug.Log(DataManager.Instance.GetParamInt ("level"));
		Debug.Log(DataManager.Instance.GetParamFloat ("health"));
		Debug.Log(DataManager.Instance.GetParamBool ("firstSession"));
	}
	
	void Update () 
	{
		
	}
	#endregion


	#region Private Methods
    #endregion
	

	#region Public Methods
    #endregion
}