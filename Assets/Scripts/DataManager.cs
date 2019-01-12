/**
 * Created by: Victoria Shenkevich
 * Created on: 12/01/2019
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour 
{
	#region Constants
	private const int TRUE = 1;
    private const int FALSE = 0;

	private const float SAVE_TO_DISK_REPEAT_RATE = 10.0f;
	#endregion


    #region Private Fields
    private Dictionary <string, object> m_data = new Dictionary<string, object>();
    #endregion


	#region Public Fields	
    #endregion


    #region Prefabs
    #endregion


    #region Singleton
    private static DataManager instance = null;

    internal static DataManager Instance 
	{
		get 
		{ 
			if (instance == null)
			{
				GameObject go = GameObject.Find ("DataManager");

				if (go != null)
				{
					instance = go.GetComponent<DataManager>();
				}
			}

			if (instance == null)
				instance = new GameObject ("DataManager").AddComponent<DataManager>();
				
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
		InvokeRepeating("SaveToDisk", SAVE_TO_DISK_REPEAT_RATE, SAVE_TO_DISK_REPEAT_RATE);
	}
	#endregion


	#region Private Methods
	private void SaveToMemory (string paramName, object param)
	{
		if (m_data.ContainsKey(paramName))
		{
			m_data[paramName] = param;
		}			
		else
		{
			m_data.Add(paramName, param);
		}
	}

	private void SaveToDisk ()
    {
        PlayerPrefs.Save();

        SaveToFile();
    }

	private void SaveToFile()
    {
        if (m_data.Count > 0)
        {
            string dataAsString = string.Empty;

            foreach (var item in m_data)
            {
                dataAsString += string.Format("{0}: {1}\n", item.Key, item.Value);
            }

            File.WriteAllText(Application.streamingAssetsPath + "/GameData.text", dataAsString);
        }
    }
    #endregion
	

	#region Public Methods
    //String
    public void SaveParam (string paramName, string param)
    {
    	SaveToMemory (paramName, param);

		PlayerPrefs.SetString(paramName, (string) param);
    }

    public string GetParamString (string paramName)
    {
    	string param = PlayerPrefs.GetString(paramName);

    	return param;
    }
    //


    //Bool
    public void SaveParam (string paramName, bool param)
    {
    	SaveToMemory (paramName, param);

   		PlayerPrefs.SetInt(paramName, (bool) param ? TRUE : FALSE);
    }

    public bool GetParamBool (string paramName)
    {
    	return (PlayerPrefs.GetInt(paramName) == TRUE ? true : false);
    }
    //


    //Int
    public void SaveParam (string paramName, int param)
    {
    	SaveToMemory (paramName, param);

    	PlayerPrefs.SetInt(paramName, param);
    }

    public int GetParamInt (string paramName)
    {
    	return PlayerPrefs.GetInt(paramName);
    }
    //


    //Float
    public void SaveParam (string paramName, float param)
    {
    	SaveToMemory (paramName, param);

    	PlayerPrefs.SetFloat(paramName, param);
    }	

    public float GetParamFloat (string paramName)
    {
    	return PlayerPrefs.GetFloat(paramName);
    }
    //
    #endregion
}