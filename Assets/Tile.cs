using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState {hidden, revealed}

public class Tile : MonoBehaviour {
	
	[SerializeField] private Sprite hidden_sprite;
	[SerializeField] private Sprite revealed_sprite;
	
	public TileState state {get; private set;}
	
	void Start () {
		this.state = TileState.hidden;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.hidden_sprite;
	}
	
	void Update () {
		
	}
	
	void OnMouseDown() {
		this.state = TileState.revealed;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.revealed_sprite;
	}
}
