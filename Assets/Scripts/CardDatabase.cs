using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
	public static List<Card> cardList = new List<Card>();

	void Awake()
	{
		cardList.Add(new Card(0, "Programación Intensa", 10, "Los programadores son bestias tímidas, hay que estimularlos. + 10 programación", Resources.Load<Sprite>("img-programacion-intensa")));
		cardList.Add(new Card(1, "Mecánica copada", 10, "Nunca antes vista, pero si lo supieras tendría que matarte. +10 diseño", Resources.Load<Sprite>("img-mecanica-copada")));
		cardList.Add(new Card(2, "Temazo", 10, "Cerati un poroto. +10 arte", Resources.Load<Sprite>("img-temazo")));
	}
}