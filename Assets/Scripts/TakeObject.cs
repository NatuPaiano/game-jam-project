using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TakeObject : MonoBehaviour
{
    private Vector2 _oldPosition;
    public bool selected;
    private SpriteRenderer _renderer;
    public Color[] possibleColors;
    public Vector2 boxCenter;
    public Vector2 boxUnlimit;

    public float timeReturn = -1;
    public float speedReturn = 1;


    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = possibleColors[Random.Range(0, possibleColors.Length)];
    }
    private void OnMouseUp()
    {
        Vector2 pos = new Vector2(transform.position.x + boxCenter.x, transform.position.y + boxCenter.y);
        var boxs = Physics2D.OverlapBoxAll(pos,boxUnlimit,0);
        print("Boxs: " + boxs.Length);
        foreach (var b in boxs)
        {
            print("b: " + b.name);
        }
        foreach (var b in boxs)
        {
            if (b.transform.gameObject.tag.Equals("Limit"))
            {
                //transform.position = _oldPosition;
                timeReturn = 1;
                break;
            }
        }
    }
    private void Update()
    {
        if(timeReturn >0)
        {
            print("TIme");
            timeReturn -= Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _oldPosition, speedReturn);
            if(timeReturn <= 0)
            {
                transform.position = _oldPosition;
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 pos = new Vector2(transform.position.x + boxCenter.x, transform.position.y + boxCenter.y);
        Gizmos.DrawWireCube(pos, boxUnlimit);
    }
    private void OnMouseDown()
    {
        _oldPosition = transform.position;
    }
}
