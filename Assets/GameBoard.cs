using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
	
	public Transform tile_prefab;
	
	private int width;
	private int height;
	private int mine_nb;
	
	private Tile[,] game_board;
	
	void Start () {
		this.width = 10;
		this.height = 10;
		this.mine_nb = 20;
		this.place_tiles();
	}
	
	void Update () {
		
	}
	
	private void place_tiles() {
		// hardcoded for now, logic later
		float offset_x = -4.5f;
		float offset_y = -6.5f;
		float size = 1f;
		
		this.game_board = new Tile[width, height];
		
		for(int col = 0; col < this.width; col++) {
			for(int row = 0; row < this.height; row++) {
				float x = offset_x + size*col;
				float y = offset_y + size*row;
				Tile new_tile = Instantiate(this.tile_prefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Tile>();
				new_tile.initialize(this, col, row);
				this.game_board[col,row] = new_tile;
			}
		}
	}
	
	private bool is_in_bounds(int col, int row) {
		return col >= 0 && col < this.width && row >= 0 && row < this.height;
	}
	
	public IEnumerable<Tile> get_neighbors(Tile origin) {
		for(int i = -1; i <= 1; i++) {
			for(int j = -1; j <= 1; j++) {
				if(i != 0 || j != 0) {
					int col = origin.col + i;
					int row = origin.row + j;
					if(this.is_in_bounds(col, row)) {
						yield return this.game_board[col,row];
					}
				}
			}
		}
	}
	
	public void place_mines(Tile origin) {
		origin.change_type(TileType.empty);
		foreach(Tile tile in this.get_neighbors(origin)) {
			tile.change_type(TileType.empty);
		}
		
		int mine_count = 0;
		while(mine_count < this.mine_nb) {
			int col = Random.Range(0, this.width);
			int row = Random.Range(0, this.height);
			if(this.game_board[col,row].change_type(TileType.mined)) {
				mine_count += 1;
			}
		}
		
		for(int col = 0; col < this.width; col++) {
			for(int row = 0; row < this.height; row++) {
				this.game_board[col,row].change_type(TileType.empty);
			}
		}
	}
	
	public void clear_neighbors(Tile origin) {
		Queue<Tile> tiles_to_clear = new Queue<Tile>();
		tiles_to_clear.Enqueue(origin);
		while(tiles_to_clear.Count > 0) {
			Tile tile = tiles_to_clear.Dequeue();
			foreach(Tile neighbor in this.get_neighbors(tile)) {
				if(neighbor.reveal()) {
					tiles_to_clear.Enqueue(neighbor);
				}
			}
		}
	}
}
