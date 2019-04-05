using System.IO;
using UnityEngine;

[System.Serializable]
public class PersistentData {

	public void Save(string filename)
	{
		Directory.CreateDirectory(Application.streamingAssetsPath);
		string path = Application.streamingAssetsPath + "/" + filename;
		string json = JsonUtility.ToJson(this, true);
		File.WriteAllText(path, json);
	}

	public void Load(string filename)
	{
		string path = Application.streamingAssetsPath + "/" + filename;
		string json = File.ReadAllText(path);
		JsonUtility.FromJsonOverwrite(json, this);
	}
}
