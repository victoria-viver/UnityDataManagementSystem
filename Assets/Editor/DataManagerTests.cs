/**
 * Created by: Victoria Shenkevich
 * Created on: 13/01/2019
 */

using UnityEngine;
using NUnit.Framework;

public class DataManagerTests : MonoBehaviour 
{
	/// <summary>
	/// Method for string saving and loading test
	/// </summary>
	[Test]
	public void StringSaveAndGetPasses ()
	{
		string username = "Victoria";
		string key = username.ToString();				

		DataManager.Instance.SaveParam(key, username);
		string returnedValue = DataManager.Instance.GetParamString (key);

		Assert.That(returnedValue, Is.EqualTo(username));
	}

	/// <summary>
	/// Method for bool saving and loading test
	/// </summary>
	[Test]
	public void BoolSaveAndGetPasses ()
	{
		bool isUserAuthorized = true;
		string key = isUserAuthorized.ToString();				

		DataManager.Instance.SaveParam(key, isUserAuthorized);
		bool returnedValue = DataManager.Instance.GetParamBool (key);

		Assert.That(returnedValue, Is.EqualTo(isUserAuthorized));
	}

	/// <summary>
	/// Method for float saving and loading test
	/// </summary>
	[Test]
	public void FloatSaveAndGetPasses ()
	{
		float winPercentage = 54.3f;
		string key = winPercentage.ToString();

		DataManager.Instance.SaveParam(key, winPercentage);
		float returnedValue = DataManager.Instance.GetParamFloat (key);

		Assert.That(returnedValue, Is.EqualTo(winPercentage));
	}

	/// <summary>
	/// Method for int saving and loading test
	/// </summary>
	[Test]
	public void IntSaveAndGetPasses ()
	{
		int score = 300;
		string key = score.ToString();

		DataManager.Instance.SaveParam(key, score);
		int returnedValue = DataManager.Instance.GetParamInt (key);

		Assert.That(returnedValue, Is.EqualTo(score));
	}


	/// <summary>
	/// Method for int saving and loading test in loop
	/// </summary>
    [Test]
    public void IntSaveAndGetInLoopPasses () 
	{
		int experience = 0;
		string key = experience.ToString();

		int count = 100;
		for (int i = 0; i < count; i++)
		{
			experience++;
			DataManager.Instance.SaveParam(key, experience);	
			int returnedValue = DataManager.Instance.GetParamInt (key);
			Assert.That(returnedValue, Is.EqualTo(experience));
		}
    } 
}