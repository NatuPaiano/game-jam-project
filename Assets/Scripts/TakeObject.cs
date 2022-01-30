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
    public Color debuffColor;
    public Vector2 boxCenter;
    public Vector2 boxUnlimit;

    public float timeReturn = -1;
    public float speedReturn = 1;

    public bool isPlayed = false;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void setColors(bool isDebuff)
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (isDebuff)
        {
            _renderer.color = debuffColor;
        }
        else
        {
            _renderer.color = possibleColors[Random.Range(0, possibleColors.Length)];
        }
    }
    public bool objectIsOnScreenLimit()
    {
        Vector2 pos = new Vector2(transform.position.x + boxCenter.x, transform.position.y + boxCenter.y);
        var boxs = Physics2D.OverlapBoxAll(pos, boxUnlimit, 0);
        foreach (var b in boxs)
        {
            if (b.transform.gameObject.tag.Equals("Limit"))
            {
                return true;
            }
        }
        return false;
    }
    public void SetTimeReturn(float f)
    {
        timeReturn = f;
    }

    //private void OnMouseUp()
    //{
    //    if(objectIsOnScreenLimit())
    //    {
    //        timeReturn = 1;
    //    }
    //}
    private void Update()
    {
        if(timeReturn >0)
        {
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
    public void SetOldPosition()
    {
        _oldPosition = transform.position;
    }
}
