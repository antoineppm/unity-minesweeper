using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Minefield {
	// empty for now
}

public class GameBoard : MonoBehaviour, Minefield {
	
	public Transform tile_prefab;
	
	private int width;
	private int height;
	
	void Start () {
		this.width = 10;
		this.height = 10;
		this.place_tiles();
	}
	
	void Update () {
		
	}
	
	
	private void place_tiles() {
		// hardcoded for now, logic later
		float offset_x = -4.5f;
		float offset_y = -6.5f;
		float size = 1f;
		
		for(int col = 0; col < this.width; col++) {
			for(int row = 0; row < this.height; row++) {
				float x = offset_x + size*col;
				float y = offset_y + size*row;
				Instantiate(this.tile_prefab, new Vector3(x, y, 0), Quaternion.identity);
			}
		}
	}
}
