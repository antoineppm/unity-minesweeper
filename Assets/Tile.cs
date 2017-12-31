using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {empty, mined}
public enum TileState {hidden, revealed}

public class Tile : MonoBehaviour {
	
	[SerializeField] private Sprite sprite_hidden;
	[SerializeField] private Sprite sprite_revealed;
	[SerializeField] private Sprite sprite_exploded;
	
	public TileType type {get; private set;}
	public TileState state {get; private set;}
	
	void Start () {
		this.type = TileType.empty;
		this.state = TileState.hidden;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_hidden;
	}
	
	void Update () {
		
	}
	
	void OnMouseDown() {
		switch(this.type) {
			case TileType.empty:
				this.state = TileState.revealed;
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_revealed;
				break;
			case TileType.mined:
				this.state = TileState.revealed;
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_exploded;
				break;
		}
	}
}
