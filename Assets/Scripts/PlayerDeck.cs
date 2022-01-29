using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> container = new List<Card>();
		public int x;
		public static int deckSize;
		public static List<Card> staticDeck = new List<Card>();
		public List<Card> deck = new List<Card>();

		public GameObject CardToHand;
		public GameObject Hand;

		public bool playCard;

		private int cardsInHandSize = 8;
    void Start()
    {
		deck = CardDatabase.cardList;
		deckSize = CardDatabase.cardList.Count;
		Shuffle();
		/*x = 0;
		deckSize = 40;
		for (int i = 0; i < deckSize; i++)
		{
			x = Random.Range(0, CardDatabase.cardList.Count);
			deck[i] = CardDatabase.cardList[x];
		}*/

		StartCoroutine(StartGame());
    }

    void Update()
    {
		staticDeck = deck;
    }
	
		private void putCardIntoHand()
		{
			Instantiate(CardToHand, transform.position, transform.rotation);
		}

		IEnumerator StartGame()
		{
			for (int i = 0; i < cardsInHandSize; i++)
			{
				yield return new WaitForSeconds(0.5f);
				//TODO: play a sound when we draw
				//audioSource.PlayOneShot(draw, 1f);
				putCardIntoHand();
			}
		}

		public void Shuffle()
		{
			for (int i = 0; i < deckSize; i++)
			{
				container[0] = deck[i];
				int randomIndex = Random.Range(i, deckSize);
				deck[i] = deck[randomIndex];
				deck[randomIndex] = container[0];	
			}
		}
}
