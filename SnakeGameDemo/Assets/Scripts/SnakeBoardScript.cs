using UnityEngine;
using System.Collections;


public class SnakeBoardScript : MonoBehaviour {

	public GameObject blockPrefab;
	public GameObject fruitPrefab;
	public float frameInterval = 0.2f;

	Direction direction = Direction.Right;
	ArrayList snakePointsList = new ArrayList(10);
	ArrayList snakeDisplayList = new ArrayList (10);
	Vector2   fruitPosition = new Vector2 (0, 0);
	GameObject fruit;
	Grid2D grid = new Grid2D(30,20);
	int unitSize = 30;
	int currentScore = 0;
	bool SpawnFruitInThisFrame = false;
	const int HEAD = 0;
	const int ID_SNAKE = 1;
	const int ID_FRUIT = 2;

	// Use this for initialization
	void Start () 
	{
		// Create default sized snake.
		ExpandSnake (2);

		SpawnFruit ();

		StartCoroutine(GameUpdate());
	}
	
	// Update is called once per frame
	void Update () {

		Direction dir = InputHandlerScript.GetMovementDirection ();
		if (dir != Direction.None)
			direction = dir;
	}

	void OnGUI () 
	{
		// Update GUI Score.
		GUI.Box (new Rect (Screen.width-180,1,150,28), "Score : "+currentScore);
	}

	IEnumerator GameUpdate ()
	{
		while (true) 
		{
			Vector2 headPosition = (Vector2)snakePointsList [HEAD];

			if (direction == Direction.Left)
				headPosition.x -= unitSize;
			else if (direction == Direction.Right)
				headPosition.x += unitSize;
			else if (direction == Direction.Top)
				headPosition.y += unitSize;
			else if (direction == Direction.Down)
				headPosition.y -= unitSize;

			// Collision checks
			Cell curCell = grid.GetCell (headPosition);
			if (curCell.border || curCell.value == ID_SNAKE)
			{
				break;	
			}
			else if (curCell.value == ID_FRUIT)
			{
				currentScore++;
				ExpandSnake (1);
				// Respawn new fruit.
				grid.Remove (fruitPosition);
				Destroy (fruit);
				SpawnFruitInThisFrame = true;
			}

			// Update snake trail
			for (int i=0; i<snakePointsList.Count; i++)
				grid.Remove ((Vector2)snakePointsList [i]);

			for (int i=snakePointsList.Count-1; i>0; i--) 
			{
				Vector2 p = (Vector2)snakePointsList [i - 1];
				grid.Add (p, 1);
				snakePointsList [i] = p;
			}

			// Update Head
			grid.Add (headPosition, ID_SNAKE);
			snakePointsList [HEAD] = headPosition;

			// Render
			RenderSnake ();

			if(SpawnFruitInThisFrame)
			{
				SpawnFruit();
				SpawnFruitInThisFrame = false;
			}
			yield return new WaitForSeconds (frameInterval);	

		}
		Application.LoadLevel (0);
	}

	bool SpawnFruit()
	{
		fruitPosition = grid.GetEmptyCellPosition ();
		grid.Add (fruitPosition,ID_FRUIT);

		Vector3 screenPosition = ConvertGridToWorld (fruitPosition);
		fruit = (GameObject)Instantiate (fruitPrefab, screenPosition, Quaternion.identity);

		return true;
	}

	void ExpandSnake(int size)
	{
		for (int i=0; i < size; i++)
		{
			Vector2 newPoint = new Vector2 (300, 300);
			snakePointsList.Add (newPoint);
			grid.Add(newPoint,ID_SNAKE);
		}
	}

	void RenderSnake()
	{
		// Remove previous snake blocks.
		for (int i=0; i < snakeDisplayList.Count; i++)
		{
			GameObject obj = (GameObject)snakeDisplayList[i];
			Destroy(obj);
		}
		snakeDisplayList.Clear();

		// Add new snake blocks.
		for (int i=0; i<snakePointsList.Count; i++) 
		{
			Vector2 p = (Vector2)snakePointsList[i]; ;
			Vector3 screenPosition = ConvertGridToWorld(p);
			GameObject block = (GameObject)Instantiate (blockPrefab, screenPosition, Quaternion.identity);
			snakeDisplayList.Add (block);
		}
	}


	Vector3 ConvertGridToWorld(Vector2 point)
	{
		Vector3 screenPosition = new Vector3 (0f, 0f, -3.5f);
		// Normalize the grid position.
		float nx = ((point.x+grid.CellSize*0.5f) / (grid.CellSize*grid.NumOfCellsPerSide));
		float ny = ((point.y+grid.CellSize*0.5f)/ (grid.CellSize*grid.NumOfCellsPerSide));

		// Convert normalized value between -1.0f to 1.0f
		screenPosition.x = 2.0f * nx - 1.0f;
		screenPosition.y = 2.0f * ny - 1.0f;

		// Scale to Ortho size.
		screenPosition.x *= Camera.main.orthographicSize * Camera.main.aspect;
		screenPosition.y *= Camera.main.orthographicSize;

		return screenPosition;
	}
}
