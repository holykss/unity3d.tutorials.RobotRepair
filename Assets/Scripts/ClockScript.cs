using UnityEngine;
using System.Collections;

public class ClockScript : MonoBehaviour {

	private bool isPaused = false;
	float startTime;
	float timeRemaining;
	
	public Texture2D clockBG;
	public Texture2D clockFG;
	public float clockFGMaxWidth;

	public Texture2D rightSide;
	public Texture2D leftSide;
	public Texture2D back;
	public Texture2D blocker;
	public Texture2D shiny;
	public Texture2D finished;


	const float totalTime = 6.0f;

	// Use this for initialization
	void Start () {
		startTime = Time.time + totalTime;
		clockFGMaxWidth = clockFG.width;
	}
	
	// Update is called once per frame
	void Update () {
		DoCountDown();

	}

	void OnGUI()
	{
		float progressRate = timeRemaining / totalTime ;
		float newBarWidth = clockFGMaxWidth * progressRate;
		const int gap = 10;

		GUI.BeginGroup(new Rect(Screen.width - clockBG.width - gap, 120, clockBG.width, clockBG.height));
		GUI.DrawTexture(new Rect(0, 0, clockBG.width, clockBG.height), clockBG);
		GUI.BeginGroup(new Rect(5, 6, newBarWidth, clockFG.height));
		GUI.DrawTexture(new Rect(0, 0, clockFG.width, clockFG.height), clockFG);
		GUI.EndGroup();
		GUI.EndGroup();


		bool isPastHafway = progressRate < .5f;

		Rect clockRect = new Rect(10, 10, 128, 128);

		GUI.DrawTexture(clockRect, back, ScaleMode.StretchToFill, true, 0);

		if (progressRate > 0.0f)
		{
			float angle = progressRate * 360.0f;
			angle *= -1;

			Vector2 centerPoint = new Vector2(clockRect.x + 64, clockRect.y + 64);
			Matrix4x4 startMatrix = GUI.matrix;

			if (isPastHafway)
			{
				GUIUtility.RotateAroundPivot(angle - 180, centerPoint);
				GUI.DrawTexture(clockRect, leftSide, ScaleMode.StretchToFill, true, 0);
				GUI.matrix = startMatrix;
				GUI.DrawTexture(clockRect, blocker, ScaleMode.StretchToFill, true, 0);

			}
			else
			{
				GUIUtility.RotateAroundPivot(angle, centerPoint);
				GUI.DrawTexture(clockRect, rightSide, ScaleMode.StretchToFill, true, 0);
				GUI.matrix = startMatrix;
				GUI.DrawTexture(clockRect, leftSide, ScaleMode.StretchToFill, true, 0);
			}

		}
		else
		{
			GUI.DrawTexture(clockRect, finished, ScaleMode.StretchToFill, true, 0);
		}

		GUI.DrawTexture(clockRect, shiny, ScaleMode.StretchToFill, true, 0);
	}

	private void DoCountDown()
	{
		if (isPaused)
			return;

		timeRemaining = startTime - Time.time;

		if (timeRemaining <= 0)
		{
			timeRemaining = 0;
			TimeIsUp();
		}

		ShowTime();

	}

	private void PauseClock()
	{
		isPaused = true;
	}

	private void UnpauseClock()
	{
		isPaused = false;
	}

	private void ShowTime()
	{
		int min;
		int sec;
		string msg;

		min = (int)timeRemaining / 60;
		sec = (int)timeRemaining % 60;
		msg = min.ToString() + ":" + sec.ToString("D2");

		guiText.text = msg;
	}

	private void TimeIsUp()
	{
		PauseClock();

		Debug.Log("Time is up!");
	}
}
