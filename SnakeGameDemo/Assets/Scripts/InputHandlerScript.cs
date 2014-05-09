using UnityEngine;
using System.Collections;

public enum Direction{Left, Right, Top, Down, None};

public class InputHandlerScript : MonoBehaviour {

	public static Direction GetMovementDirection()
	{
		Direction direction = Direction.None;
		bool bLeftKeyPressed = Input.GetKeyDown (KeyCode.LeftArrow);
		bool bRightKeyPressed = Input.GetKeyDown (KeyCode.RightArrow);
		bool bUpKeyPressed = Input.GetKeyDown (KeyCode.UpArrow);
		bool bDownKeyPressed = Input.GetKeyDown (KeyCode.DownArrow);

		if (bLeftKeyPressed)
			direction = Direction.Left;
		else if (bRightKeyPressed)
			direction = Direction.Right;
		else if (bUpKeyPressed)
			direction = Direction.Top;
		else if (bDownKeyPressed)
			direction = Direction.Down;

		return direction;
	}
}
