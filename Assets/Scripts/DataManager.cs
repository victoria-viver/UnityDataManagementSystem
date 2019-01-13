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
	private Dictionary <string, object> m_dataDictionary = new Dictionary<string, object>();
	private StorageType m_storageType = StorageType.Memory;
	private string m_dataFilePath;    
	private bool m_isLoadingFromFileFinished = true;
    #endregion


	#region Public Fields
	public enum StorageType {Memory, PlayerPrefs, File}; //Only one can be chosen	
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
	/// <summary>
	/// Saves data to memory (dictionary).
	/// Updates entry if already exists, otherwise adds a new one.
	/// </summary>
	/// <param name="paramName">Used to specify entry's to save name</param>
	/// <param name="param">Used to specify entry's to save value</param>
    private void SaveToMemory (string paramName, object param)
	{
		if (m_dataDictionary.ContainsKey(paramName))
		{
			m_dataDictionary[paramName] = param;
		}			
		else
		{
			m_dataDictionary.Add(paramName, param);
		}
	}

	/// <summary>
	/// Creates string from memory data dictionary and saves it to a text file
	/// </summary>
	private void SaveToFile()
    {
        if (m_isLoadingFromFileFinished && m_dataDictionary.Count > 0)
        {
            string dataAsString = string.Empty;

            foreach (var item in m_dataDictionary)
            {
                dataAsString += string.Format("{0}:{1}\n", item.Key, item.Value);
            }

            File.WriteAllText(m_dataFilePath, dataAsString);
        }
    }

	/// <summary>
	/// Loads data from file if exists.
	/// Prevents data lose on file resave.
	/// </summary>
	private void LoadFromFile()
    {
		if (File.Exists(m_dataFilePath))
		{
			m_isLoadingFromFileFinished = false;

			string text = System.IO.File.ReadAllText(m_dataFilePath);
			string[] data = text.Split (new Char [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
			int entriesCount = data.Length;

			for (int i = 0; i < entriesCount; i++)
			{
				//TODO: find solution for preventing this symbol use in keys 
				string[] line = data[i].Split (new Char [] {':'}, StringSplitOptions.RemoveEmptyEntries);

				if (!string.IsNullOrEmpty(line[KEY]))
				{
					//Add or update memory data with loaded from a file data
					SaveToMemory(line[KEY], line[VALUE]);
				}				
			}

			m_isLoadingFromFileFinished = true;
		}
    }
    #endregion
	

	#region Public Methods
	/// <summary>
	/// User can specify StorageType
	/// </summary>
	/// <param name="storageType">Used to specify storage type</param>
	public void SetStorageType (StorageType storageType)
	{
		m_storageType = storageType;

		OnStorageTypeSet ();
	}

	/// <summary>
	/// Called once storage type was changed
	/// </summary>
    private void OnStorageTypeSet()
    {
		//if storage type is a file system, it's needed to load the data from the file to memory
        if (m_storageType == StorageType.File)
		{
			LoadFromFile();
		}
    }

    //String
	/// <summary>
	/// Saves got string
	/// </summary>
	/// <param name="paramName">String to save name</param>
	/// <param name="param">String to save value</param>
    public void SaveParam (string paramName, string param)
    {
		//Always save to memory for faster access
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.Memory:
				break;
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetString(paramName, param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogWarning("This StorageType isn't handled");
				break;
		}
    }

	/// <summary>
	/// Returns requested string
	/// </summary>
	/// <param name="paramName">Requested string name</param>
	/// <param name="defaultValue">Optional - used for default value if entry isn't found</param>
	/// <returns></returns>
    public string GetParamString (string paramName, string defaultValue = "")
    {
		string param = defaultValue;

		if (m_dataDictionary.ContainsKey(paramName))
		{
			param = (string) m_dataDictionary[paramName];
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetString(paramName);

			//Save to memory for current session future faster reuse
			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Bool
	/// <summary>
	/// Saves got bool
	/// </summary>
	/// <param name="paramName">Bool to save name</param>
	/// <param name="param">Bool to save value</param>
    public void SaveParam (string paramName, bool param)
    {
		//Always save to memory for faster access
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.Memory:
				break;
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetInt(paramName, param ? TRUE : FALSE);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogWarning("This StorageType isn't handled");
				break;
		}
    }

	/// <summary>
	/// Returns requested bool
	/// </summary>
	/// <param name="paramName">Requested bool name</param>
	/// <param name="defaultValue">Optional - used for default value if entry isn't found</param>
	/// <returns></returns>
    public bool GetParamBool (string paramName, bool defaultValue = false)
    {
		bool param = defaultValue;

		if (m_dataDictionary.ContainsKey(paramName))
		{
			param = (bool) m_dataDictionary[paramName];
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = (PlayerPrefs.GetInt(paramName) == TRUE ? true : false);

			//Save to memory for current session future faster reuse
			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Int
	/// <summary>
	/// Saves got int
	/// </summary>
	/// <param name="paramName">Int to save name</param>
	/// <param name="param">Int to save value</param>
    public void SaveParam (string paramName, int param)
    {
		//Always save to memory for faster access
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.Memory:
				break;
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetInt(paramName, param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogWarning("This StorageType isn't handled");
				break;
		}
    }

	/// <summary>
	/// Returns requested int
	/// </summary>
	/// <param name="paramName">Requested int name</param>
	/// <param name="defaultValue">Optional - used for default value if entry isn't found</param>
	/// <returns></returns>
    public int GetParamInt (string paramName, int defaultValue = 0)
    {
    	int param = defaultValue;

		if (m_dataDictionary.ContainsKey(paramName))
		{
			param = Convert.ToInt32(m_dataDictionary[paramName]);
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetInt(paramName);

			//Save to memory for current session future faster reuse
			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //


    //Float
	/// <summary>
	/// Saves got float
	/// </summary>
	/// <param name="paramName">Float to save name</param>
	/// <param name="param">Float to save value</param>
    public void SaveParam (string paramName, float param)
    {
		//Always save to memory for faster access
    	SaveToMemory (paramName, param);

		switch (m_storageType)
		{
			case StorageType.Memory:
				break;
			case StorageType.PlayerPrefs:
				PlayerPrefs.SetFloat(paramName, param);
				PlayerPrefs.Save();
				break;
			case StorageType.File:
				SaveToFile();
				break;
			default:
				Debug.LogWarning("This StorageType isn't handled");
				break;
		}
    }	

	/// <summary>
	/// Returns requested float
	/// </summary>
	/// <param name="paramName">Requested float name</param>
	/// <param name="defaultValue">Optional - used for default value if entry isn't found</param>
	/// <returns></returns>
    public float GetParamFloat (string paramName, float defaultValue = 0.0f)
    {
    	float param = defaultValue;

		if (m_dataDictionary.ContainsKey(paramName))
		{
			param = (float) Convert.ToDecimal(m_dataDictionary[paramName]);
		}			
		else if (m_storageType == StorageType.PlayerPrefs)
		{
			param = PlayerPrefs.GetFloat(paramName);
			
			//Save to memory for current session future faster reuse
			SaveToMemory(paramName, param);
		}
		
		return param;
    }
    //
    #endregion
}