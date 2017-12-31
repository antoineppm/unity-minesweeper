using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {init, empty, mined}
public enum TileState {hidden, revealed}

public class Tile : MonoBehaviour {
	
	[SerializeField] private Sprite sprite_hidden;
	[SerializeField] private Sprite sprite_revealed;
	[SerializeField] private Sprite sprite_exploded;
	
	private GameBoard parent;
	
	public int col {get; private set;}
	public int row {get; private set;}
	
	public TileType type {get; private set;}
	public TileState state {get; private set;}
	
	void Start () {
		this.type = TileType.init;
		this.state = TileState.hidden;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_hidden;
	}
	
	public void initialize(GameBoard parent, int col, int row) {
		this.parent = parent;
		this.col = col;
		this.row = row;
	}
	
	void Update () {
		
	}
	
	void OnMouseDown() {
		switch(this.type) {
			case TileType.init:
				this.parent.place_mines(this);
				break;
			case TileType.empty:
				this.state = TileState.revealed;
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_revealed;
				break;
			case TileType.mined:
				this.state = TileState.revealed;
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_exploded;
				break;
			default:
				break;
		}
	}
	
	public bool change_type(TileType type) {
		if(this.type == TileType.init) {
			this.type = type;
			return true;
		} else {
			return false;
		}
	}
}
