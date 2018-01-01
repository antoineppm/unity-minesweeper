using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {init, empty, mined}
public enum TileState {hidden, revealed, flagged, locked}

public class MineTile : MonoBehaviour {
	
	[SerializeField] private Sprite sprite_hidden;
	[SerializeField] private Sprite sprite_flagged;
	[SerializeField] private Sprite[] sprite_revealed;
	[SerializeField] private Sprite sprite_exploded;
	[SerializeField] private Sprite sprite_mined;
	[SerializeField] private Sprite sprite_wrongflag;
	
	private static int empty_hidden_nb = 0;
	
	private MineManager parent;
	
	public int col {get; private set;}
	public int row {get; private set;}
	
	public TileType type {get; private set;}
	public TileState state {get; private set;}
	
	void Start () {
		this.type = TileType.init;
		this.state = TileState.hidden;
		this.change_sprite(this.sprite_hidden);
	}
	
	public void initialize(MineManager parent, int col, int row) {
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
						this.change_sprite(this.sprite_flagged);
					} else if(this.state == TileState.flagged) {
						this.state = TileState.hidden;
						this.change_sprite(this.sprite_hidden);
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
			if(type == TileType.empty) {
				empty_hidden_nb++;
			}
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
				foreach(MineTile tile in parent.get_neighbors(this)) {
					if(tile.type == TileType.mined) {
						neighbor_nb++;
					}
				}
				this.change_sprite(this.sprite_revealed[neighbor_nb]);
				empty_hidden_nb--;
				if(empty_hidden_nb == 0) {
					parent.game_over(true);
				}
				return (neighbor_nb == 0);
			case TileType.mined:
				this.state = TileState.revealed;
				this.change_sprite(this.sprite_exploded);
				parent.game_over(false);
				break;
			default:
				break;
		}
		return false;
	}
	
	public void lockdown(bool victory) {
		if(victory && this.type == TileType.mined) {
			this.change_sprite(this.sprite_flagged);
		}
		if(!victory && this.type == TileType.mined && this.state == TileState.hidden) {
			this.change_sprite(this.sprite_mined);
		}
		if(!victory && this.type == TileType.empty && this.state == TileState.flagged) {
			this.change_sprite(this.sprite_wrongflag);
		}
		this.state = TileState.locked;
	}
	
	private void change_sprite(Sprite sprite) {
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	}
}
