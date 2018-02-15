using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSquare : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler {
	eTileState state = eTileState.empty;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter (PointerEventData eventData) {
		if (GameStateScript.mouse_state == ePlayerState.mouseDown) {
			if (GameStateScript.tile_set_state == eTileState.on && state == eTileState.empty)
				set_on ();
			else if (GameStateScript.tile_set_state == eTileState.off && state == eTileState.empty)
				set_off ();
			else if (GameStateScript.tile_set_state == eTileState.empty && state == GameStateScript.tile_clear_state)
				set_empty ();
		}
	}

	public void OnPointerDown (PointerEventData eventData) {
		// Put the player state in mouseDown mode
		GameStateScript.mouse_state = ePlayerState.mouseDown;

		if (eventData.button == PointerEventData.InputButton.Left) {
			if (state == eTileState.empty)
				set_on (true);
			else if (state == eTileState.on)
				set_empty (true, state);
		}
		else if (eventData.button == PointerEventData.InputButton.Right) {
			if (state == eTileState.empty)
				set_off (true);
			else if (state == eTileState.off)
				set_empty (true, state);
		}

		// Middle Click (If we ever need it)
		// else if (eventData.button == PointerEventData.InputButton.Middle)
	}

	public void OnPointerUp (PointerEventData eventData) {
		// Clean up our states
		GameStateScript.mouse_state = ePlayerState.idle;
		GameStateScript.tile_set_state = eTileState.none;
		GameStateScript.tile_clear_state = eTileState.none;
	}
		
	void set_on(bool click=false) {
		SpriteRenderer item = this.gameObject.GetComponent<SpriteRenderer>();
		state = eTileState.on;
		item.color = Color.blue;

		if (click)
			GameStateScript.tile_set_state = state;
	}

	void set_empty(bool click=false, eTileState clear_state=eTileState.none) {
		SpriteRenderer item = this.gameObject.GetComponent<SpriteRenderer>();
		state = eTileState.empty;
		item.color = Color.white;

		if (click) {
			GameStateScript.tile_set_state = state;
			GameStateScript.tile_clear_state = clear_state;
		}
	}

	void set_off(bool click=false) {
		SpriteRenderer item = this.gameObject.GetComponent<SpriteRenderer>();
		state = eTileState.off;
		item.color = Color.red;

		if (click)
			GameStateScript.tile_set_state = state;
	}
}
