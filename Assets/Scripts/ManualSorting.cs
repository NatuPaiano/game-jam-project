using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManualSorting : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;
    [SerializeField]
    public int offset = 0;
    [SerializeField]
    private bool runOnlyOnce = false;
    public bool childWithSprites = false;
    private Tuple<int, SpriteRenderer>[] childs;
    private float timer;
    private float timerMax = .1f;
    private Renderer myRenderer;
    public bool inverse = true;
    public Canvas worldCanvas;


    void Start()
    {
        if (childWithSprites == false) 
            myRenderer = gameObject.GetComponent<Renderer>();
        else
        {
            var aux = this.transform.GetComponentsInChildren<SpriteRenderer>(true);
            List<Tuple<int, SpriteRenderer>> auxList = new List<Tuple<int, SpriteRenderer>>();
            foreach (var a in aux)
            {
                auxList.Add(new Tuple<int, SpriteRenderer>( a.sortingOrder, a));
            }
            childs = auxList.ToArray();
        }
        if(inverse)
        {
            foreach (var c in childs)
            {
                c.Item2.sortingOrder *= -1;
            }
            worldCanvas.sortingOrder *= -1;
        }
    }
    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            float correctionNumber = transform.position.y * 100;
            if (childWithSprites == false)
            {
                myRenderer.sortingOrder = (int)(sortingOrderBase - correctionNumber - offset);
                worldCanvas.sortingOrder = (int)(sortingOrderBase - correctionNumber - offset);
                if (inverse)
                {
                    myRenderer.sortingOrder *= -1;
                    worldCanvas.sortingOrder *= -1;
                }
            }
            else
            {
                foreach (var c in childs)
                {
                    c.Item2.sortingOrder = (int)(c.Item1+sortingOrderBase - correctionNumber - offset);
                    worldCanvas.sortingOrder = (int)(c.Item1 + sortingOrderBase - correctionNumber - offset);
                    if (inverse)
                    {
                        c.Item2.sortingOrder *= -1;
                        worldCanvas.sortingOrder *= -1;
                    }
                }
            }
            timer = timerMax;

            if (runOnlyOnce || gameObject.isStatic == true)
                enabled = false;
        }
    }
    public void Reajust()
    {
        float correctionNumber = transform.position.y * 100;
        if (childWithSprites == false)
            myRenderer.sortingOrder = (int)(sortingOrderBase - correctionNumber - offset);
        else
        {
            foreach (var c in childs)
            {
                c.Item2.sortingOrder = (int)(c.Item1 + sortingOrderBase - correctionNumber - offset);
            }
        }
        timer = timerMax;
    }
}
