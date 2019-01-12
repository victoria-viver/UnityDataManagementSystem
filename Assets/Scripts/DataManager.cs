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

	private const int KEY = 0;
	private const int VALUE = 1;

    private const string DATA_FILE_NAME = "Data.text";
    #endregion


    #region Private Fields
	private string m_dataFilePath;
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

		m_dataFilePath = Path.Combine(Application.streamingAssetsPath, DATA_FILE_NAME);
	}

	void Start () 
	{
		LoadFromFile();
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

	private void SaveToFile()
    {
        if (m_data.Count > 0)
        {
            string dataAsString = string.Empty;

            foreach (var item in m_data)
            {
                dataAsString += string.Format("{0}:{1}\n", item.Key, item.Value);
            }

            File.WriteAllText(m_dataFilePath, dataAsString);
        }
    }

	private void LoadFromFile()
    {
		if (File.Exists(m_dataFilePath))
		{
			string text = System.IO.File.ReadAllText(m_dataFilePath);
			string[] data = text.Split (new Char [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
			int entriesCount = data.Length;

			for (int i = 0; i < entriesCount; i++)
			{
				string[] line = data[i].Split (new Char [] {':'}, StringSplitOptions.RemoveEmptyEntries);

				SaveToMemory(line[KEY], line[VALUE]);
			}		
		}
    }
    #endregion
	

	#region Public Methods
    //String
    public void SaveParam (string paramName, string param)
    {
    	SaveToMemory (paramName, param);

		PlayerPrefs.SetString(paramName, (string) param);
		PlayerPrefs.Save();

		SaveToFile();
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
		PlayerPrefs.Save();

		SaveToFile();
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
		PlayerPrefs.Save();

		SaveToFile();
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
		PlayerPrefs.Save();

		SaveToFile();
    }	

    public float GetParamFloat (string paramName)
    {
    	return PlayerPrefs.GetFloat(paramName);
    }
    //
    #endregion
}