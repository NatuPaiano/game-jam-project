using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayCard : MonoBehaviour
{
	//UI
	public TextMeshProUGUI title;
	public TextMeshProUGUI description;
	public SpriteRenderer artwork;

	public Note noteData;
	/*public List<Note> displayCard = new List<Note>();
	public int displayId;*/

	private int id;
	public int programming;
	public int art;
	public int design;
	public int totalProgress;
	public int noteToDelete;

	public bool isRandomCard = true;

	//public int numberOfCardsInDeck;

	void Start()
	{
		if(isRandomCard)
        {
			getCardData();
        }
			fillUICardData();
	}

	void Update()
	{

	}
	public void getCardData(Note noteModel)
	{
		isRandomCard = false;
		noteData = noteModel;
		id = noteData.id;
		programming = noteData.programming;
		art = noteData.art;
		design = noteData.design;

		GetComponent<TakeObject>().setColors(noteData.isDebuff);
	}
	void getCardData()
	{
		int cardIndex = Random.Range(0, PlayerDeck.deckSize);
		noteData = PlayerDeck.staticDeck[cardIndex];
		getCardData(noteData);
	}

	void fillUICardData ()
    {
		name = noteData.title;
		title.text = noteData.title;
		description.text = noteData.description;
		artwork.sprite = noteData.artwork;
	} 
}
