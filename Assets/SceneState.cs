using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; // The System.IO namespace contains functions related to loading and saving files


public class SceneState : MonoBehaviour {

	public string level_file = "";
	int[,] grid_data;

	// Use this for initialization
	void Start () {
		LoadGameData ();
		LoadTiles ();		
	}

	void LoadTiles() {
		for (int i = 0; i < grid_data.GetLength(0); i++) {
			for (int j = 0; j < grid_data.GetLength(1); j++) {

			}
		}
	}

	[Serializable]
	public class LevelData
	{
		public int[] data;
		public int length;
		public int height;
	}

	void print_grid() {
		string output = "";
		for (int i = 0; i < grid_data.GetLength(0); i++) {
			output += "[";
			for (int j = 0; j < grid_data.GetLength(1); j++) {
				output += grid_data [i, j] + ", ";
			}
			output += "]\r\n";
		}
		Debug.Log(output);
	}

	private void LoadGameData()
	{
		// Path.Combine combines strings into a file path
		// Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
		string filePath = Path.Combine(Application.streamingAssetsPath, "levels");
		filePath = Path.Combine(filePath, level_file + ".json");

		Debug.Log (filePath);
		if(File.Exists(filePath))
		{
			// Read the json from the file into a string
			string dataAsJson = File.ReadAllText(filePath); 
			// Pass the json to JsonUtility, and tell it to create a GameData object from it
			LevelData loadedData = JsonUtility.FromJson<LevelData>(dataAsJson);
			setGridData (loadedData);
		}
		else
		{
			Debug.LogError("Cannot load game data!");
		}
	}

	void setGridData(LevelData loadedData) {
		grid_data = new int[loadedData.length, loadedData.height];
		int count = 0;
		for (int i = 0; i < loadedData.length; i++) {
			for (int j = 0; j < loadedData.height; j++) {
				grid_data [i, j] = loadedData.data [count++];
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
