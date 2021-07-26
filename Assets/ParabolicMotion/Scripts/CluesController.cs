using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluesController : MonoBehaviour {

	[SerializeField] private AudioSource newClue;
	[SerializeField] private AudioSource emptyClue;
	[SerializeField] private GameObject[] clues;
	private int currentClue = 0;

	void Start() {
		ResetClues();
	}

	public void NextClue() {
		if (currentClue < clues.Length - 1) {
			newClue.Play();
			clues[currentClue++].SetActive(false);
			clues[currentClue].SetActive(true);
		}
		else {
			emptyClue.Play();
		}
	}

	public void ResetClues() {
		clues[0].SetActive(true);
		for (int i = 1; i < clues.Length; i++) {
			clues[i].SetActive(false);
		}

		currentClue = 0;
	}

}
