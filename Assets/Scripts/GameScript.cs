using UnityEngine;
using System.Collections;
using System;

public class GameScript : MonoBehaviour
{

	const int cols = 4;
	const int rows = 4;
	const int totalCards = cols * rows;
	const int matchesNeededToWin = (int)(totalCards * 0.5);
	const int cardW = 100;
	const int cardH = 100;

	int matchesMade = 0;
	ArrayList aCards;
	Card[][] aGrid;
	ArrayList aCardsFlipped;
	bool playerCanClick;
	bool playerHasWon = false;




	// Use this for initialization
	void Start()
	{
		this.playerCanClick = true;

		this.aCards = new ArrayList();
		this.aGrid = new Card[rows][];

		this.aCardsFlipped = new ArrayList();

		BuildDeck();

		for (int i = 0; i < rows; i++)
		{
			aGrid[i] = new Card[cols];

			for (int j = 0; j < cols; j++)
			{
				int someNum = UnityEngine.Random.Range(0, aCards.Count);
				aGrid[i][j] = (Card)aCards[someNum];
				aCards.RemoveAt(someNum);
			}
		}


	}

	// Update is called once per frame
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
	
		BuildGrid();
		if (playerHasWon)
			BuildWinPrompt();

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
//		print("building grid!");

		//         int buttonW = 100;
		//         int buttonH = 50;
		// 
		//         float halfScreenW = Screen.width / 2;
		//         float halfButtonW = buttonW / 2;
		// 
		// 
		//         if (GUI.Button(new Rect(halfScreenW - halfButtonW, Screen.height / 3 * 2, buttonW, buttonH), "Play Game"))
		//         {
		//             print("game");
		//         }

	}

	private void BuildWinPrompt()
	{
		int winPromptW = 100;
		int winPromptH = 90;

		int halfPromptW = (int)(winPromptW * .5);
		int halfPromptH = (int)(winPromptH * .5);

		float halfScreenW = Screen.width * .5f;
		float halfScreenH = Screen.height* .5f;

		GUI.BeginGroup(new Rect(halfScreenW - halfPromptW, halfScreenH - halfPromptH, winPromptW, winPromptH));

		GUI.Box(new Rect(0, 0, winPromptW, winPromptH), "A Winner is You@!");

		if (GUI.Button(new Rect(10, 40, 80, 20), "Play Again"))
		{
			Application.LoadLevel("title");
		}

		GUI.EndGroup();
	}

	void FixedUpdate()
	{

	}

	private void BuildGrid()
	{
		GUILayout.BeginVertical();
		for (int i = 0; i < rows; i++)
		{
			GUILayout.BeginHorizontal();

			for (int j = 0; j < cols; j++)
			{
				Card card = aGrid[i][j];

				string imageName;

				if (card.isMatched)
				{
					imageName = "blank";				
				}
				else if (card.isFaceUp)
				{
					imageName = card.img;
				}
				else
				{
					imageName = "wrench";
				}

				GUI.enabled = !card.isMatched;

				Texture tex = (Texture)Resources.Load(imageName);

				if (GUILayout.Button(tex, GUILayout.Width(cardW)))
				{
					if (playerCanClick)
						StartCoroutine(FlipCardFaceUp(card));

//					Debug.Log(card.img);
				}

				GUI.enabled = true;

			}

			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}

	private IEnumerator FlipCardFaceUp(Card card)
	{
		card.isFaceUp = true;
		
		if (aCardsFlipped.IndexOf(card) < 0)
			aCardsFlipped.Add(card);

		if (aCardsFlipped.Count == 2)
		{
			playerCanClick = false;

			yield return new WaitForSeconds(1);

			Card card1 = (Card)aCardsFlipped[0];
			Card card2 = (Card)aCardsFlipped[1];

			if (card1.id == card2.id)
			{
				card1.isMatched = true;
				card2.isMatched = true;
				matchesMade++;

				if (matchesMade >= matchesNeededToWin)
				{
					playerHasWon = true;
				}
			}
			else
			{
				card1.isFaceUp = false;
				card2.isFaceUp = false;
			}

			aCardsFlipped.Clear();

			playerCanClick = true;

		}
	}

	private void BuildDeck()
	{
		const int totalRobots = 4;
		Card card;
		int id = 0;

		for (int i = 0; i < totalRobots; i++)
		{
			ArrayList aRobotParts = new ArrayList(new string[] {"Head", "Arm", "Leg" });

			for (int j = 0; j < 2; j++)
			{
				int someNum = UnityEngine.Random.Range(0, aRobotParts.Count);
				string theMissingPart = (string)aRobotParts[someNum];

				aRobotParts.RemoveAt(someNum);

				card = new Card("robot" + (i + 1) + "Missing" + theMissingPart, id);
				aCards.Add(card);

				card = new Card("robot" + (i + 1) + theMissingPart, id);
				aCards.Add(card);

				id++;
			}
		}
	}

}

class Card : System.Object
{
	public bool isFaceUp = false;
	public bool isMatched = false;
	public int id;
	public string img = null;

	public Card(string img, int id)
	{
		this.img = img;
		this.id = id;

	}

}