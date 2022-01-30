using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int programmingProgress = 0;
    public int artProgress = 0;
    public int designProgress = 0;
    public float totalProgress = 0;

    public int programmingGoal = 40;
    public int artGoal = 40;
    public int designGoal = 50;
    private int totalGoalValue = 0;

    public int playedDays = 0;
    public int cardsToNextDay = 4;
    public List<GameObject> totalPlayedCards = new List<GameObject>();
    public List<GameObject> dayPlayedCards = new List<GameObject>();
    public GameObject debuffNoteModel;
    public Canvas gameCanvas;

    void Start()
    {
        programmingGoal = Random.Range(30, 70);
        artGoal = Random.Range(30, 70);
        designGoal = Random.Range(30, 70);
        totalGoalValue = programmingGoal + artGoal + designGoal;
    }

    void Update()
    {
        
    }

    public void sumGoalProgress(Note note)
    {
        programmingProgress += note.programming;
        artProgress += note.art;
        designProgress += note.design;

        programmingProgress = Mathf.Clamp(programmingProgress, 0, programmingGoal);
        artProgress = Mathf.Clamp(artProgress, 0, artGoal);
        designProgress = Mathf.Clamp(designProgress, 0, designGoal);

        totalProgress = (float)(programmingProgress + artProgress + designProgress) / (float)totalGoalValue;
    }

    public void addPlayedNote(GameObject playedNote)
    {
        dayPlayedCards.Add(playedNote);
        totalPlayedCards.Add(playedNote);
        generateDebuff(playedNote.GetComponent<DisplayCard>().noteData);
    }

   void startNextDay()
    {
        FindObjectOfType<PlayerDeck>().cleanHand();
        dayPlayedCards.Clear();
        StartCoroutine(FindObjectOfType<PlayerDeck>().StartGame());
    }

    public void checkIfNextDayIsOn()
    {
        if (dayPlayedCards.Count >= cardsToNextDay)
        {
            startNextDay();
        }
    }

    public void generateDebuff(Note playedNote)
    { 
        if(playedNote.debuffQuantity.Length <= 0)
        {
            return;
        }
        
        foreach (var item in playedNote.debuffQuantity)
        {
            var currentPlayedCardList = item.isDailyDebuff ? dayPlayedCards : totalPlayedCards;
            var auxList = currentPlayedCardList.Select(item => item.GetComponent<DisplayCard>().noteData).ToList();
            int neededIncompatibleCards = 0;
            foreach (var note in item.incompatibleCards)
            {
                Note foundNote = null;
                foreach (var aux in auxList)
                {
                    if (note == aux)
                    {
                        neededIncompatibleCards++;
                        foundNote = aux;
                        break;
                    }
                }
                if(foundNote != null)
                {
                    auxList.Remove(foundNote);
                }
            }
            if(neededIncompatibleCards >= item.incompatibleCards.Length)
            {
                var debuffCard = Instantiate(debuffNoteModel, transform.position, transform.rotation, gameCanvas.transform);
                debuffCard.GetComponent<DisplayCard>().getCardData(item.generatedDebuff);
                sumGoalProgress(debuffCard.GetComponent<DisplayCard>().noteData);


                /*if(item.destroysNote.Length > 0)
                {
                    //get cards to delete
                    var playedCards = totalPlayedCards;
                    var cardsToDeleteIds = new List<int>();
                    var totalPlayedCardsIds = totalPlayedCards.Select((playedCard) => playedCard.GetComponent<DisplayCard>().noteData.id).ToList();
                    var destroysNoteIds = item.destroysNote.Select((destroyNote) => destroyNote.id).ToList();
                    cardsToDeleteIds = totalPlayedCardsIds.Intersect(destroysNoteIds).ToList();

                    //delete them
                    var objectsToDestroyList = new List<GameObject>();
                    for (int i = 0; i < totalPlayedCards.Count; i++)
                    {
                        if(cardsToDeleteIds.Contains(totalPlayedCards[i].GetComponent<DisplayCard>().noteData.id))
                        {
                            objectsToDestroyList.Add(totalPlayedCards[i]);
                        }
                    }
                    foreach (var obj in objectsToDestroyList)
                    {
                        if (totalPlayedCards.Contains(obj))
                        { 
                            Destroy(obj);
                            totalPlayedCards.Remove(obj);
                        }
                    }
                }*/
            }
        }
    }
}
