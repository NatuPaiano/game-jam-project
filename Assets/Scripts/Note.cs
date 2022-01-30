using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "Note", menuName = "Note")]
public class Note: ScriptableObject
{
	public int id;
	public string title;
	[TextArea]
	public string description;
	public int programming;
	public int art;
	public int design;
	public Sprite artwork;
	public bool isDebuff;
	public DebuffNote[] debuffQuantity;
}

[System.Serializable]
public class DebuffNote
{
	public Note[] incompatibleCards;
	public Note generatedDebuff;
	public Note[] destroysNote;
	public bool isDailyDebuff;
}
