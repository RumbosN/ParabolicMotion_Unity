using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrieController : MonoBehaviour {

	[SerializeField] private Sprite EmptyTrieImg;
	[SerializeField] private Sprite PendingTrieImg;
	[SerializeField] private Sprite SuccessTrieImg;
	[SerializeField] private Sprite WrongTrieImg;
	[SerializeField] private Image[] Tries;

	private int currentTrie;
	private Dictionary<ETrieStatus, Sprite> statusToImageMap;

    void Start() {
	    currentTrie = 0;
		statusToImageMap = new Dictionary<ETrieStatus, Sprite>() {
			{ ETrieStatus.EMPTY, EmptyTrieImg},
			{ ETrieStatus.INPROGRESS, PendingTrieImg},
			{ ETrieStatus.SUCCESS, SuccessTrieImg},
			{ ETrieStatus.FAILED, WrongTrieImg}
		};
		SetCurrentTrieTo(ETrieStatus.INPROGRESS);
    }

    private void SetCurrentTrieTo(ETrieStatus newStatus) {
	    Tries[currentTrie].sprite = statusToImageMap[newStatus];
    }

    private void SetCurrentAndNext(ETrieStatus newStatus) {
	    SetCurrentTrieTo(newStatus);

	    if (currentTrie < Tries.Length - 1) {
			currentTrie++;
			SetCurrentTrieTo(ETrieStatus.INPROGRESS);
	    }
    }

    public void SuccessAndNext() {
	    SetCurrentAndNext(ETrieStatus.SUCCESS);
    }
}
