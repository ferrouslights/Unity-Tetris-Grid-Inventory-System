using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
	public interface ICursorInput
	{
		public Vector2 CursorPosition { get; }
		public bool CursorLocked { get; set; }
		public bool CursorInputForLook { get; set; }
	}
}
