using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO; // The System.IO namespace contains functions related to loading and saving files


public class SceneState : MonoBehaviour {

	public string level_file = "";
	public GameObject square_prefab;

	int[,] grid_data;
	GameObject[,] grid_squares;
	List<int>[] rows;
	List<int>[] columns; 
	int x = 0;
	int y = 0;

	// Use this for initialization
	void Start () {
		LoadGameData ();
		LoadTiles ();		
	}

	void output_rows() {
		string output = "All Rows\r\n";
		for (int i = 0; i < y ; i++) {
			output += "Row [" + i + "]: " + String.Join (" ", rows[i].Select(x => x.ToString()).ToArray()) + "\r\n";
		}
		Debug.Log (output);
	}

	void output_columns() {
		string output = "All Cols\r\n";
		for (int i = 0; i < x ; i++) {
			output += "Col [" + i + "]: " + String.Join (" ", columns [i].Select (x => x.ToString ()).ToArray ()) + "\r\n";
		}
		Debug.Log (output);
	}

	void LoadTiles() {
		int offset = 1;
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				Vector3 pos = new Vector3 ((offset * i) - (x / 2), (offset * j) - (y / 2), 1);
				grid_squares [i, j] = Instantiate (square_prefab, pos, Quaternion.identity);
			}
		}

		// Performance: This is garbage, we need to make it better and/or faster somehow
		int row_counter = 0;
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if (grid_data [i, j] == 1) {
					row_counter++;
				} 
				if (grid_data [i, j] == 0 || j == y - 1) {
					if (row_counter != 0)
						rows[i].Add(row_counter);
					row_counter = 0;
				}
			}
		}
		int column_counter = 0;
		for (int i = 0; i < y; i++) {
			for (int j = 0; j < x; j++) {
				if (grid_data [j, i] == 1) {
					column_counter++;
				}
				if (grid_data[j, i] == 0 || j == x - 1) {
					if (column_counter != 0 )
						columns[i].Add(column_counter);
					column_counter = 0;
				}
			}
		}
			
		output_rows ();
		output_columns ();
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
		x = loadedData.length;
		y = loadedData.height;

		// Initialize our arrays
		grid_data = new int[x, y];
		grid_squares = new GameObject[x, y];
		columns = new List<int>[x];
		rows = new List<int>[y];
	
		// Put our data into our grid_data rray
		int count = 0;
		for (int i = 0; i < x; i++) {
			columns [i] = new List<int> ();
			for (int j = 0; j < y; j++) {
				grid_data [i, j] = loadedData.data [count++];
				rows [j] = new List<int> ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
