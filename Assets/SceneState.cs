using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.IO; // The System.IO namespace contains functions related to loading and saving files


public class SceneState : MonoBehaviour {

	public string level_file = "";
	public GameObject square_prefab;
	public Canvas canvas;

	int[,] grid_data;
	GameObject[,] grid_squares;
	List<int>[] row_clues;
	List<int>[] column_clues; 
	int length = 0;
	int height = 0;

	// Use this for initialization
	void Start () {
		LoadPuzzleData ();
		FindPuzzleClues ();
		LoadTiles ();
		LoadClues ();
	}

	void output_rows() {
		string output = "All Rows\r\n";
		for (int i = 0; i < height ; i++) {
			output += "Row [" + i + "]: " + String.Join (" ", row_clues[i].Select(x => x.ToString()).ToArray()) + "\r\n";
		}
		Debug.Log (output);
	}

	void output_columns() {
		string output = "All Cols\r\n";
		for (int i = 0; i < length ; i++) {
			output += "Col [" + i + "]: " + String.Join (" ", column_clues [i].Select (x => x.ToString ()).ToArray ()) + "\r\n";
		}
		Debug.Log (output);
	}

	void LoadTiles() {
		int offset = 1;
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < height; j++) {
				Vector3 pos = new Vector3 ((offset * i) - (i * 0.5f) - (length / 4), (offset * j)  - (j * 0.5f) - (height / 4), 1);
				grid_squares [i, j] = Instantiate (square_prefab, pos, Quaternion.identity);
			}
		}
	}

	void LoadClues(){
		int fontsize = 20;

		string str = "";

		for (int i = 0; i < row_clues.Length; i++) {
			for (int j = row_clues[i].Count - 1 ; j >= 0 ; j--) {
				GameObject ngo = new GameObject ("myTextGO");
				ngo.transform.SetParent (canvas.transform);
				Text myText = ngo.AddComponent<Text> ();
				myText.color = Color.black;
				myText.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
				myText.fontSize = fontsize;
				myText.text = row_clues [i] [j].ToString ();
				RectTransform rt = myText.GetComponent<RectTransform> ();
				rt.position = new Vector3 (-2.3f - ((row_clues[i].Count - j) * 0.2f), 1.5f - i + (i * 0.5f), 1);
				rt.localScale = new Vector3 (1, 1, 1);
				rt.sizeDelta = new Vector2 (myText.preferredWidth, myText.preferredHeight);
			}
		}

		for (int i = 0; i < column_clues.Length; i++) {
			for (int j = column_clues [i].Count - 1; j >= 0; j--) {
				GameObject ngo = new GameObject ("myTextGO");
				ngo.transform.SetParent (canvas.transform);
				Text myText = ngo.AddComponent<Text> ();
				myText.color = Color.black;
				myText.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
				myText.fontSize = fontsize;
				myText.text = column_clues [i] [j].ToString ();;
				RectTransform rt = myText.GetComponent<RectTransform> ();
				rt.position = new Vector3 (-2 + i - (i * 0.5f), 1.9f + ((column_clues[i].Count - j) * 0.2f), 1);
				rt.localScale = new Vector3 (1, 1, 1);
				rt.sizeDelta = new Vector2 (myText.preferredWidth, myText.preferredHeight);
			}
		}
	}
		
	void FindPuzzleClues() {
		// Performance: This is garbage, we need to make it better and/or faster somehow
		int row_counter = 0;
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < height; j++) {
				if (grid_data [i, j] == 1) {
					row_counter++;
				} 
				if (grid_data [i, j] == 0 || j == height - 1) {
					if (row_counter != 0)
						row_clues[i].Add(row_counter);
					row_counter = 0;
				}
			}
		}
		int column_counter = 0;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < length; j++) {
				if (grid_data [j, i] == 1) {
					column_counter++;
				}
				if (grid_data[j, i] == 0 || j == length - 1) {
					if (column_counter != 0 )
						column_clues[i].Add(column_counter);
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

	private void LoadPuzzleData()	{
		// Path.Combine combines strings into a file path
		// Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
		string filePath = Path.Combine(Application.streamingAssetsPath, "levels");
		filePath = Path.Combine(filePath, level_file + ".json");

		if (File.Exists(filePath)) {
			// Read the json from the file into a string
			string dataAsJson = File.ReadAllText(filePath); 
			// Pass the json to JsonUtility, and tell it to create a GameData object from it
			LevelData loadedData = JsonUtility.FromJson<LevelData>(dataAsJson);
			setGridData (loadedData);
		} else {
			Debug.LogError("Cannot load game data!");
		}
	}

	void setGridData(LevelData loadedData) {
		length = loadedData.length;
		height = loadedData.height;

		// Initialize our arrays
		grid_data = new int[length, height];
		grid_squares = new GameObject[length, height];
		column_clues = new List<int>[length];
		row_clues = new List<int>[height];
	
		// Put our data into our grid_data rray
		int count = 0;
		for (int i = 0; i < length; i++) {
			column_clues [i] = new List<int> ();
			for (int j = 0; j < height; j++) {
				grid_data [i, j] = loadedData.data [count++];
				row_clues [j] = new List<int> ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
