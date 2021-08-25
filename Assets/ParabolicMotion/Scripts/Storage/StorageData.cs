using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StorageData {

	public List<PlayerData> Users;

	public StorageData() {
		Users = new List<PlayerData>();
	}

	public string ToJson() {
		return JsonUtility.ToJson(this);
	}

	public void LoadFromJson(string json) {
		JsonUtility.FromJsonOverwrite(json, this);
	}
}
