using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StorageData {

	public List<PlayerData> Users;

	public string ToJson() {
		return JsonUtility.ToJson(this);
	}

	public void LoadFromJson(string json) {
		JsonUtility.FromJsonOverwrite(json, this);
	}
}
