using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();
		public int displayId;

		public int id;
		public string cardName;
		public int power;
		public string cardDescription;
		public Sprite spriteImage;

//UI
		public Text nameText;
		public Text powerText;
		public Text descriptionText;
		public Image artImage;

		//public int numberOfCardsInDeck;

		void Start()
		{
			//numberOfCardsInDeck = PlayerDeck.deckSize;

			displayCard[0] = CardDatabase.cardList[displayId];
		}

		void Update()
		{
			id = displayCard[0].id;
			cardName = displayCard[0].cardName;
			power = displayCard[0].power;
			cardDescription = displayCard[0].cardDescription;
			spriteImage = displayCard[0].spriteImage;

			nameText.text = " " + cardName;
			powerText.text = " " + power;
			descriptionText.text = " " + cardDescription;
			artImage.sprite = spriteImage;

			if (this.tag == "InHand")
			{
				int cardIndex = Random.Range(0, PlayerDeck.deckSize);
				displayCard[0] = PlayerDeck.staticDeck[cardIndex];
				//numberOfCardsInDeck -= 1;
				//PlayerDeck.deckSize -= 1;
				this.tag = "Untagged";
			}
		}
}
