using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	public static int totalScore = 0;
	private Text myText;
	
	void Start(){
		myText = GetComponent<Text>();
		myText.text = totalScore.ToString();
	}
	
	public void Score(int points){
		totalScore += points;
		myText.text = totalScore.ToString();
	}
	
	public static void Reset(){
		totalScore = 0;
	}
}


