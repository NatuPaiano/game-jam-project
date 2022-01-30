using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TakeManager : MonoBehaviour
{
    public GameObject selectedPiece;
    public PlayerDeck playerDeck;
    public GameManager gameManager;
    public float verticalCorrection = -5;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerDeck = FindObjectOfType<PlayerDeck>();  
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits.Length <= 0)
                return;
            var take = hits.Where(x => x.transform.CompareTag("Note"))
                        .OrderByDescending(x => x.transform.GetComponent<SpriteRenderer>().sortingOrder)
                        .Select(x => x.transform.gameObject)
                        .FirstOrDefault();
            if(take != null)
            {
                selectedPiece = take;
                selectedPiece.GetComponent<TakeObject>().selected = true;
            }

            //if (hits.transform.CompareTag("Note"))
            //{
            //    if (!hit.transform.GetComponent<TakeObject>().fit)
            //    {
            //        selectedPiece = hit.transform.gameObject;
            //        selectedPiece.GetComponent<TakeObject>().selected = true;
            //    }
            //}

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedPiece != null)
            {
                selectedPiece.GetComponent<TakeObject>().selected = false;
                if (!selectedPiece.GetComponent<TakeObject>().objectIsOnScreenLimit() && !selectedPiece.GetComponent<TakeObject>().isPlayed)
                {
                    if (!selectedPiece.GetComponent<DisplayCard>().noteData.isDebuff) 
                    {
                        gameManager.addPlayedNote(selectedPiece);
                        gameManager.sumGoalProgress(selectedPiece.GetComponent<DisplayCard>().noteData);
                    }
                    selectedPiece.GetComponent<TakeObject>().isPlayed = true;
                    playerDeck.removeCardFromDeck(selectedPiece);
                    // assign selected piece parent to be CANVAS
                    selectedPiece.transform.parent = selectedPiece.transform.parent.parent;
                }
                selectedPiece = null;
            }
        }
        if (selectedPiece != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPiece.transform.position = new Vector3(mousePos.x, mousePos.y + verticalCorrection, 0);
        }
    }
}
