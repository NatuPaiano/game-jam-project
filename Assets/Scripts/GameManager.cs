using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;

    public int programmingProgress = 0;
    public int artProgress = 0;
    public int designProgress = 0;
    public float totalProgress = 0;

    public int programmingGoal = 40;
    public int artGoal = 40;
    public int designGoal = 50;
    private int totalGoalValue = 0;

    public int currentDay = 1;
    public int totalDays = 7;
    public int cardsToNextDay = 4;
    public List<GameObject> totalPlayedCards = new List<GameObject>();
    public List<GameObject> dayPlayedCards = new List<GameObject>();
    public GameObject debuffNoteModel;
    public Canvas gameCanvas;
    public AudioClip[] songs;


    //UI
    public TextMeshProUGUI currentDayDisplay;
    public TextMeshProUGUI currentProgrammingProgress;
    public TextMeshProUGUI currentArtProgress;
    public TextMeshProUGUI currentDesignProgress;
    public TextMeshProUGUI currentProjectGoalProgress;
    public Image black;


    private void Awake()
    {
        songs = Resources.LoadAll<AudioClip>("Sounds/Songs");
    }

    public BoxCollider2D respawnDebuffArea;

    void Start()
    {
        currentDayDisplay.text = currentDay.ToString() + "/" + totalDays.ToString();

        programmingGoal = Random.Range(70, 110);
        artGoal = Random.Range(70, 110);
        designGoal = Random.Range(70, 110);
        totalGoalValue = programmingGoal + artGoal + designGoal;

        audioManager.PlayMusic(songs[currentDay-1], 0.15f);

        StartCoroutine(DelayBlack());
    }

    void Update()
    {
        currentProgrammingProgress.text = programmingProgress.ToString() + " / " + programmingGoal;
        currentArtProgress.text = artProgress.ToString() + " / " + artGoal;
        currentDesignProgress.text = designProgress.ToString() + " / " + designGoal;
        currentProjectGoalProgress.text =  Mathf.RoundToInt(totalProgress * 100).ToString() + " %";
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
        audioManager.StopMusic();

        currentDay++;
        currentDayDisplay.text = currentDay.ToString() + "/" + totalDays.ToString();

        StartCoroutine(DelayBlack());
        if (currentDay > totalDays)
        {
            endGame();
            return;
        }

        FindObjectOfType<PlayerDeck>().cleanHand();
        dayPlayedCards.Clear();
        StartCoroutine(FindObjectOfType<PlayerDeck>().StartGame());


        if(currentDay < songs.Length)
            audioManager.PlayMusic(songs[currentDay-1], 0.15f);
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
            var auxList = currentPlayedCardList.Select(item => new Tuple<ParticlesFeedback, Note> (item.GetComponent<ParticlesFeedback>(),item.GetComponent<DisplayCard>().noteData)).ToList();
            int neededIncompatibleCards = 0;

            foreach (var note in item.incompatibleCards)
            {
                Tuple<ParticlesFeedback, Note> foundNote = null;
                foreach (var aux in auxList)
                {
                    if (note == aux.Item2)
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
            if (neededIncompatibleCards >= item.incompatibleCards.Length)
            {
                Vector2 respawnPossition = new Vector2(
                                              Random.Range(respawnDebuffArea.bounds.min.x, respawnDebuffArea.bounds.max.x),
                                              Random.Range(respawnDebuffArea.bounds.min.y, respawnDebuffArea.bounds.max.y)
                                              );
                var debuffCard = Instantiate(debuffNoteModel, respawnPossition, transform.rotation, gameCanvas.transform);
                debuffCard.GetComponent<DisplayCard>().getCardData(item.generatedDebuff);
                debuffCard.GetComponent<TakeObject>().isPlayed = true;
                debuffCard.GetComponent<ParticlesFeedback>().BadRespawnFB();
                debuffCard.transform.localScale *= .65f;
                AudioManager.Instance.PlaySFX("Sounds/SFX/arrugar", 1);
                sumGoalProgress(debuffCard.GetComponent<DisplayCard>().noteData);

                //foreach (var fb in auxList)
                //{
                //    fb.Item1.ResponsabilityFB();
                //}


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
    public void endGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(totalProgress > .9f ? "GameWin" : "GameOver");
    }
    IEnumerator DelayBlack()
    {
        float alpha = 1;
        black.color = Color.black;
        while(black.color.a > 0)
        {
            black.color = new Color(0,0,0,alpha);
            yield return new WaitForSeconds(.1f);
            alpha -= .1f;
        }
    }
}
