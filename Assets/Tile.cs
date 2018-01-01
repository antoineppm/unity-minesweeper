using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {init, empty, mined}
public enum TileState {hidden, revealed, flagged}

public class Tile : MonoBehaviour {
	
	[SerializeField] private Sprite sprite_hidden;
	[SerializeField] private Sprite sprite_flagged;
	[SerializeField] private Sprite[] sprite_revealed;
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
	
	void OnMouseDown() {
		if(this.type == TileType.init) {
			this.parent.place_mines(this);
		}
		if(this.reveal()) {
			this.parent.clear_neighbors(this);
		}
	}
	
	void OnMouseOver() {
		// kludge because there's no right click event for some reason
		if(Input.GetMouseButtonDown(1)) {
			switch(this.type) {
				case TileType.empty:
				case TileType.mined:
					if(this.state == TileState.hidden) {
						this.state = TileState.flagged;
						gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_flagged;
					} else if(this.state == TileState.flagged) {
						this.state = TileState.hidden;
						gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_hidden;
					}
					break;
				default:
					break;
			}
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
	
	public bool reveal() {
		if(this.state != TileState.hidden) {
			return false;
		}
		switch(this.type) {
			case TileType.empty:
				this.state = TileState.revealed;
				// count neighbors
				int neighbor_nb = 0;
				foreach(Tile tile in parent.get_neighbors(this)) {
					if(tile.type == TileType.mined) {
						neighbor_nb++;
					}
				}
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_revealed[neighbor_nb];
				return (neighbor_nb == 0);
			case TileType.mined:
				this.state = TileState.revealed;
				gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite_exploded;
				break;
			default:
				break;
		}
		return false;
	}
}
