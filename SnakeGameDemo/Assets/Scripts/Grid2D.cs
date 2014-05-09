using UnityEngine;
using System.Collections;

public struct Cell
{
	public int x;
	public int y;
	public int value;
	public bool border;

	public Cell(int _x, int _y)
	{
		x = _x;
		y = _y;
		value = 0;
		border = false;
	}
};

public class Grid2D {

	int cellSize = 0;
	int numOfCellsPerSide = 0;
	const int BORDER = 2;
	Cell[] cells;

	public Grid2D(int size, int numOfcells)
	{
		cellSize = size;
		numOfCellsPerSide = numOfcells + BORDER;
		cells = new Cell[numOfCellsPerSide * numOfCellsPerSide];

		// Create grid.
		for (int r=0; r<numOfCellsPerSide; r++)
		{
			for (int c=0; c<numOfCellsPerSide; c++)
			{
				int x = (int)(c*cellSize);
				int y = (int)(r*cellSize);
				Cell newTile = new Cell(x,y);
				cells[c + numOfCellsPerSide * r] = newTile;

				// Mark border cells.
				if(c == 0 || c == numOfCellsPerSide-1 || r == 0 || r == numOfCellsPerSide -1)
					cells[c + numOfCellsPerSide * r].border = true;
			}
		}
	}

	public int CellSize
	{
		get
		{
			return cellSize;
		}
	}

	public int NumOfCellsPerSide
	{
		get
		{
			return numOfCellsPerSide;
		}
	}

	public void Add(Vector2 position, int value)
	{
		int index = PositionToIndex (position);
		cells [index].value = value;
	}

	public void Remove(Vector2 position)
	{
		int index = PositionToIndex (position);
		cells [index].value = 0;
	}

	public int PositionToIndex(Vector2 position)
	{
		int index = (int)position.x / cellSize + ((int)position.y / cellSize) * numOfCellsPerSide;
		return index;
	}

	public Cell GetCell(Vector2 position)
	{
		int index = PositionToIndex (position);
		return cells[index];
	}

	public Vector2 GetEmptyCellPosition()
	{
		bool findEmptyCell = true;
		Vector2 freePosition = new Vector2();
		int totalCells = numOfCellsPerSide * numOfCellsPerSide;

		while (findEmptyCell) 
		{
			int randomIndex = Random.Range(0,totalCells-1);

			if(cells[randomIndex].border == false && cells[randomIndex].value == 0 )
			{
				findEmptyCell = false;
				freePosition.x = cells[randomIndex].x;
				freePosition.y = cells[randomIndex].y;
				break;
			}
		}

		return freePosition;
	}
}
