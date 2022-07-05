using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {

        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one PathFinding " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

       

    }


    public void SetUp(int width, int height, float cellSize) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //creat a grid system which made from path node
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

       // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);


        //path finding obstacle system
        for(int x = 0; x < width; x++) 
        {
           for(int z = 0;z < height; z++) 
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                //cast a raycast from center down a bit to top to detect if there's a obstacle
                float raycastOffsetDistance = 5f;
               if( Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, 
                                Vector3.up, 
                                raycastOffsetDistance * 2,
                                obstaclesLayerMask)) 
                {

                    GetNode(x, z).SetIsWalkable(false);
                }

            
            }
        
        }

    }



    public List<GridPosition> FindPath(GridPosition startGridPosition,GridPosition endGridPosition,out int pathLength) 
    {
        List<PathNode> openList = new List<PathNode>(); //contained all the nodes are queued for searching
        List<PathNode> closedList = new List<PathNode>();//contained all the already searched nodes

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        //cycle through all the  path nodes in the grid system , initialzing all the nodes on path finding
        for(int x =0; x < gridSystem.GetWidth();x++) 
        {
           for(int z = 0; z < gridSystem.GetHeight(); z++) 
            {
                GridPosition gridPosition = new GridPosition(x, z);
                //get the path node object
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPath();
               
            }
        
        }

        startNode.SetGCost(0);
        startNode.SetGCost(CalculateDistance(startGridPosition,endGridPosition));
        startNode.CalculateFCost();

        //if openlist still have elements inside
        while (openList.Count > 0) 
        {
            //grab the current node, which is going to be the one on the list that has the lowest F cost
            PathNode currentNode = GetLowestFcostPathNode(openList);

            //check if current node is final node
            if(currentNode == endNode) 
            {

                //prevent moving too long distance even there's obstacle
                pathLength = endNode.GetFcost();

                //reach the final nod
                return CalculatePath(endNode);
            
            }

            //if its not final node,mark this node has already been searched
            openList.Remove(currentNode);
            closedList.Add(currentNode);



            //check all the neighbour on this node
            foreach(PathNode neighbourNode in GetNeighbourList(currentNode)) 
            {
                //check if the neighbour has already been searched
                if(closedList.Contains(neighbourNode)) 
                {
                    continue;
                }

                //check if this neighbour node walkable , if not ,add them to already searched list and skip
                if (!neighbourNode.IsWalkable()) 
                {
                    closedList.Add(neighbourNode);
                    continue;
                }


                //calculate the Gcost from start node to this current selected node
                int tentativeGCost = currentNode.GetGcost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
            

                //if this node cost less than all its neighbour node
                if(tentativeGCost < neighbourNode.GetGcost()) 
                {
                    //we find a better path to go into this neighbor node from our current node
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) 
                    {

                        openList.Add(neighbourNode);
                    }
                
                }
            }

        }

        //no path found
        pathLength = 0;
        return null;
    
    }


    public int CalculateDistance(GridPosition gridPositionA,GridPosition gridPositionB) 
    {

        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int totalDistance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance,zDistance) + MOVE_STRAIGHT_COST * remaining;
    }


    private PathNode GetLowestFcostPathNode(List<PathNode> pathNodeList) 
    {
        PathNode lowestFcostPathNode = pathNodeList[0];
        for(int i = 0; i < pathNodeList.Count; i++) 
        {
            if (pathNodeList[i].GetFcost() < lowestFcostPathNode.GetFcost()) 
            {

                lowestFcostPathNode = pathNodeList[i];
            }
        
        }

        return lowestFcostPathNode;
    
    }


    private PathNode GetNode(int x, int z) 
    {
       return  gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) 
    {
        List<PathNode> neighbourList = new List<PathNode>();

        //add all neightbour pathnodes
        GridPosition gridPosition = currentNode.GetGridPosition();


        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

      
        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourList;
    
    }


    private List<GridPosition> CalculatePath(PathNode endNode) 
    {
        List<PathNode> pathNodeList = new List<PathNode>();

        //start from final node
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;
        //check if there's any node link to it
        while (currentNode.GetCameFromPathNode() != null) 
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodeList) 
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    
    }

    //getter for whether a grid is walkable
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
       return gridSystem.GetGridObject(gridPosition).IsWalkable();

    }

    //setter to make sure we can update whether a grid is walkable
    public void SetIsWalkableGridPosition(GridPosition gridPosition,bool isWalkable) 
    {
         gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    
    }


    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)  
    {
        //check if there' a path to potential grid
       return FindPath(startGridPosition, endGridPosition, out int pathLength) !=null;
                
    }


    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition) 
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
       
    }


}
