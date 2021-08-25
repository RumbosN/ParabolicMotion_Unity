using Oculus.Platform;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _successLevelImage;
    [SerializeField] private GameObject _blockedLevelImage;
    [SerializeField] private GameObject _currentLevelImage;

    public void SetStatus(ELevelStatus status) {
	    _button.interactable = (status != ELevelStatus.BLOCKED);

        _successLevelImage.SetActive(status == ELevelStatus.SUCCESSFUL);
        _blockedLevelImage.SetActive(status == ELevelStatus.BLOCKED);
        _currentLevelImage.SetActive(status == ELevelStatus.CURRENT);
    }
}
