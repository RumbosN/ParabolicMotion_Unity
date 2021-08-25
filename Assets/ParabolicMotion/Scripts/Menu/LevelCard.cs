using Oculus.Platform;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _successLevelImage;
    [SerializeField] private Sprite _blockedLevelImage;
    [SerializeField] private Sprite _currentLevelImage;

    public void SetStatus(ELevelStatus status) {
	    _button.interactable = (status != ELevelStatus.BLOCKED);

	    switch (status) {
            case ELevelStatus.BLOCKED: _image.sprite = _blockedLevelImage; break;
            case ELevelStatus.CURRENT: _image.sprite = _currentLevelImage; break;
            case ELevelStatus.SUCCESSFUL: _image.sprite = _successLevelImage; break;
	    }
    }
}
