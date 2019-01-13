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
	private StorageType m_storageType;
	private string m_dataFilePath;
    private Dictionary <string, object> m_data = new Dictionary<string, object>();
    #endregion


	#region Public Fields
	public enum StorageType {PlayerPrefs, File}; //Only one can be chosen	
    #endregion


    #region Prefabs
    #endregion


    #region Singleton
    private static DataManager instance = null;

    public static DataManager Instance 
	{
		get 
		{ 
			if (instance == null)
				instance = (DataManager) FindObjectOfType(typeof(DataManager));	

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
				//TODO: find solution for preventing this symbol use in keys 
				string[] line = data[i].Split (new Char [] {':'}, StringSplitOptions.RemoveEmptyEntries);

				if (!string.IsNullOrEmpty(line[KEY]))
				{
					SaveToMemory(line[KEY], line[VALUE]);
				}				
			}		
		}
    }
    #endregion
	

	#region Public Methods
	public void SetStorageType (StorageType storageType)
	{
		m_storageType = storageType;

		OnStorageTypeSet ();
	}

    private void OnStorageTypeSet()
    {
        if (m_storageType == StorageType.File)
		{
			LoadFromFile();
		}
    }

    //String
    public void SaveParam (string paramName, string param)
    {
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetString(paramName, (string) param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogError("StorageType isn't set");
				break;
		}
    }

    public string GetParamString (string paramName, string defaultValue = "")
    {
		string param = defaultValue;

		if (m_data.ContainsKey(paramName))
		{
			param = (string) m_data[paramName];
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetString(paramName);

			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Bool
    public void SaveParam (string paramName, bool param)
    {
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetInt(paramName, (bool) param ? TRUE : FALSE);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogError("StorageType isn't set");
				break;
		}
    }

    public bool GetParamBool (string paramName, bool defaultValue = false)
    {
		bool param = defaultValue;

		if (m_data.ContainsKey(paramName))
		{
			param = (bool) m_data[paramName];
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = (PlayerPrefs.GetInt(paramName) == TRUE ? true : false);

			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Int
    public void SaveParam (string paramName, int param)
    {
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetInt(paramName, param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogError("StorageType isn't set");
				break;
		}
    }

    public int GetParamInt (string paramName, int defaultValue = 0)
    {
    	int param = defaultValue;

		if (m_data.ContainsKey(paramName))
		{
			param = Convert.ToInt32(m_data[paramName]);
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetInt(paramName);

			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Float
    public void SaveParam (string paramName, float param)
    {
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetFloat(paramName, param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogError("StorageType isn't set");
				break;
		}
    }	

    public float GetParamFloat (string paramName, float defaultValue = 0.0f)
    {
    	float param = defaultValue;

		if (m_data.ContainsKey(paramName))
		{
			param = (float) Convert.ToDecimal(m_data[paramName]);
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetFloat(paramName);

			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //
    #endregion
}