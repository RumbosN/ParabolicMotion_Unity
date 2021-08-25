using System;

[Serializable]
public class PlayerData {

	public string UserName;
	public int CurrentLevel;

	public PlayerData(string username, int currentLevel) {
		UserName = username;
		CurrentLevel = currentLevel;
	}
}
