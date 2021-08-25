using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

	private string persistentPath;

	void Start() {
		persistentPath = $"{Application.persistentDataPath}{Path.AltDirectorySeparatorChar}{Constants.StorageDataFile}";
	}

	public void SaveData(int currentLevel) {
		StorageData data = LoadJsonData();

		int lastLevel = Math.Max(currentLevel, DUserData.LastSuccessLevel);
		string username = DUserData.Username;
		DUserData.LastSuccessLevel = lastLevel;
		PlayerData user = data?.Users?.FirstOrDefault(x => x.UserName == username);

		if (user != null) {
			user.CurrentLevel = lastLevel;
		}
		else {
			if (data?.Users == null) {
				data = new StorageData();
			}

			data.Users.Add(new PlayerData(username, lastLevel));
		}

		FileUtils.WriteToFile(persistentPath, data.ToJson());
	}

	public StorageData LoadJsonData() {
		if (FileUtils.LoadFromFile(persistentPath, out var json)) {
			StorageData storageData = new StorageData();
			storageData.LoadFromJson(json);
			return storageData;
		}

		return null;
	}

	void OnApplicationQuit() {
		Debug.Log($"Application ending after {Time.time} seconds");
		SaveData(DUserData.LastSuccessLevel);
	}
}
