public static class GameStateScript {
	public static ePlayerState mouse_state = ePlayerState.idle;
	public static eTileState tile_set_state = eTileState.none;
	public static eTileState tile_clear_state = eTileState.none;

	public static string to_string() {
		return string.Format ("{0}, {1}, {2}", mouse_state, tile_set_state, tile_clear_state);
	}
}
