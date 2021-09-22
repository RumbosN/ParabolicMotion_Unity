using System.Collections;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager> {

	[SerializeField] private GameObject _videoObject;
	[SerializeField] private float _videoTime;
	[SerializeField] private float _videoTimeHide;

	[SerializeField] private GameObject _loginCanvas;
	[SerializeField] private GameObject _levelsCanvas;
	[SerializeField] private LevelCard[] _levelCards;

	void Start() {
		_videoObject.SetActive(true);
		_loginCanvas.SetActive(false);
		_levelsCanvas.SetActive(false);
		StartCoroutine(DisappearedVideo());
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

	protected IEnumerator DisappearedVideo() {
		yield return new WaitForSeconds(_videoTime);

		StartCoroutine(HideVideo());

		Material material = _videoObject.GetComponent<MeshRenderer>().material;
		Color color = material.color;
		color.a = 0;
		material.DOColor(color, _videoTimeHide);
	}

	protected IEnumerator HideVideo() {
		yield return new WaitForSeconds(_videoTimeHide);
		_videoObject.SetActive(false);
		ShowLogin(true);
	}
}
