using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager> {

	[SerializeField] private GameObject _loginCanvas;
	[SerializeField] private GameObject _levelsCanvas;
	[SerializeField] private LevelCard[] _levelCards;

	void Start() {
		ShowLogin(true);
	}

	public void Login(TMP_InputField usernameField) {
		StorageData data = DataManager.instance.LoadJsonData();
		string username = usernameField.text;

		PlayerData user = data?.Users?.FirstOrDefault(x => x.UserName == username);

		DUserData.Username = username;
		DUserData.LastSuccessLevel = user?.CurrentLevel ?? -1;

		LoadLevels();
	}

	public void LoadLevels() {
		ShowLogin(false);
		ELevelStatus status;

		for (int i = 0; i < _levelCards.Length; i++) {
			if (i < DUserData.LastSuccessLevel + 1) {
				status = ELevelStatus.SUCCESSFUL;
			} else if (i == DUserData.LastSuccessLevel + 1) {
				status = ELevelStatus.CURRENT;
			}
			else {
				status = ELevelStatus.BLOCKED;
			}

			_levelCards[i].SetStatus(status);
		}
	}

	private void ShowLogin(bool show) {
		_loginCanvas.SetActive(show);
		_levelsCanvas.SetActive(!show);
	}

	public void ChangeToLevel(string level) {
		SceneManager.LoadScene(level);
	}
}
