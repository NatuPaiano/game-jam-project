using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDeck : MonoBehaviour
{
	public static int deckSize;
	public static Note[] staticDeck;
	private List<GameObject> currentDeck = new List<GameObject>();

	public GameObject cardModel;
	public GameObject Hand;

	private int cardsInHandSize = 6;
    void Awake()
    {
		staticDeck = Resources.LoadAll<Note>("ScriptableObjects").Where(note => !note.isDebuff).ToArray();
		deckSize = staticDeck.Length;
		//TODO: ver esto!!!
		//deck = CardDatabase.cardList;
		//Shuffle();
		/*x = 0;
		for (int i = 0; i < deckSize; i++)
		{
			x = Random.Range(0, CardDatabase.cardList.Count);
			deck[i] = CardDatabase.cardList[x];
		}*/

		StartCoroutine(StartGame());
    }

    void Update()
    {
		//staticDeck = deck;
    }
	
	private void putCardIntoHand()
	{
		var cardInstance = Instantiate(cardModel, transform.position, transform.rotation, Hand.transform);
		currentDeck.Add(cardInstance.gameObject);
	}

	public IEnumerator StartGame()
	{
		for (int i = 0; i < cardsInHandSize; i++)
		{
			yield return new WaitForSeconds(0.5f);
			//TODO: play a sound when we draw
			//audioSource.PlayOneShot(draw, 1f);
			putCardIntoHand();
		}
	}

	public void cleanHand()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
			Destroy(currentDeck[i]);
        }
		currentDeck.Clear();
	}

	public void removeCardFromDeck(GameObject cardToRemove)
    {
		currentDeck.Remove(cardToRemove);
		FindObjectOfType<GameManager>().checkIfNextDayIsOn();
    }

	/*public void Shuffle()
	{
		for (int i = 0; i < deckSize; i++)
		{
			container[0] = deck[i];
			int randomIndex = Random.Range(i, deckSize);
			deck[i] = deck[randomIndex];
			deck[randomIndex] = container[0];	
		}
	}*/
}
