using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : System.IComparable<Node>, System.IEquatable<Node>
{
	public int x, y;
	public Node parent;
	public int gScore;
	public int hScore;
	public int nCost;

	public Node(int _x, int _y, int _c)
	{
		this.x = _x;
		this.y = _y;
		gScore = 0;
		hScore = 0;
		nCost = _c;
	}

	public Node(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
		gScore = 0;
		hScore = 0;
		nCost = 1;
	}

	public int GetScore()
	{
		return gScore + hScore;
	}

	public int CompareTo(Node obj)
	{
		Node n = (Node)obj;
		int cFactor = this.GetScore() - n.GetScore();
		return cFactor;
	}

	public bool Equals(Node obj)
	{
		Node n = (Node)obj;

		if (this.x == n.x && this.y == n.y)
			return true;

		return false;
	}

	public void CalculateScores(int _x, int _y)
	{
		if (parent == null)
			gScore = nCost;
		else
			gScore = parent.gScore + nCost;

		if (parent == null)
			hScore = 0;
		else
			hScore = parent.hScore + EstimateFunc(_x, _y);
	}

	int EstimateFunc(int _x, int _y)
	{
		int xd = _x - this.x;
		int yd = _y - this.y;

		return System.Math.Abs(System.Math.Abs(xd) + System.Math.Abs(yd));
	}
}


public class AStar
{

	public int mapWidth;
	public int mapHeight;

	List<Node> openList;
	List<Node> closedList;

	public List<Node> finalPath;


	public Node startNode;
	public Node goalNode;

	public bool isDiagonalMovementAllowed;
	public bool isNodeCostEnabled;

	private int _cancelCount;


	public AStar()
	{
		openList = new List<Node>();
		closedList = new List<Node>();
	}

	public void SetMapSize(int w, int h)
	{
		mapWidth = w;
		mapHeight = h;
	}

	public void SetStartNode(int x, int y)
	{
		startNode = new Node(x, y);
		startNode.parent = null;

	}

	public void SetGoalNode(int x, int y)
	{
		goalNode = new Node(x, y);
	}

	public void GetPath()
	{
		finalPath = new List<Node>();
		Node p = goalNode;

		while (p != null)
		{
			//int tileNo = (p.y * mapWidth) + p.x;
			finalPath.Add(p);
			p = p.parent;
		}

	}

	public int GetGoalScore()
	{
		goalNode.CalculateScores(goalNode.x, goalNode.y);
		return goalNode.gScore;
	}

	public void StartSearch(List<int> tileMap)
	{
		openList.Clear();
		closedList.Clear();

		openList.Add(startNode);

		_cancelCount = 99999;

		while (openList.Count > 0)
		{
			_cancelCount -= 1;
			if (_cancelCount < 0)
				break;

			Node currentNode = openList[0];
			openList.RemoveAt(0);

			if (currentNode.Equals(goalNode))
			{
				goalNode.parent = currentNode.parent;
				break;
			}


			for (int i = (currentNode.x - 1); i < (currentNode.x + 2); i++)
			{
				if ((i < 0) || (i >= mapWidth))
					continue;

				for (int j = (currentNode.y - 1); j < (currentNode.y + 2); j++)
				{
					if ((j < 0) || (j >= mapHeight))
						continue;

					if (tileMap[(j * mapWidth) + i] <= 0)
						continue;

					if (isDiagonalMovementAllowed == false)
					{
						if (Mathf.Abs(i - currentNode.x) == Mathf.Abs(j - currentNode.y))
							continue;
					}

					Node successorNode = null;
					if (isNodeCostEnabled == true)
						successorNode = new Node(i, j, tileMap[(j * mapWidth) + i]);
					else
						successorNode = new Node(i, j);
					successorNode.parent = currentNode;
					successorNode.CalculateScores(goalNode.x, goalNode.y);

					int oFound = openList.IndexOf(successorNode);

					if (oFound > 0)
					{
						if (openList[oFound].CompareTo(currentNode) <= 0)
							continue;
					}

					int cFound = closedList.IndexOf(successorNode);

					//if node_successor is on the CLOSED list 
					//but the existing one is as good
					//or better then discard this successor and continue;
					if (cFound > 0)
					{
						if (closedList[cFound].CompareTo(currentNode) <= 0)
							continue;
					}

					//Remove occurences of node_successor from OPEN and CLOSED
					if (oFound != -1)
						openList.RemoveAt(oFound);
					if (cFound != -1)
						closedList.RemoveAt(cFound);

					openList.Add(successorNode);
				}

			}


			openList.Sort();
			closedList.Add(currentNode);
		}

	}

}
