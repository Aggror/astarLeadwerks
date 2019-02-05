using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeadwerksEngine;

namespace AStartTest
{
    public class Pathfinder
    {
        public Tile[,] grid = new Tile[AStartTest.gridWidth, AStartTest.gridHeight];
        TVec2 startTile;
        TVec2 endTile;
        TVec2 currentTile;

        //Create the lists that store allready checked tiles
        List<TVec2> closedList = new List<TVec2>();
        List<TVec2> openList = new List<TVec2>();

        public Pathfinder( Tile[,] grid)
        {
            this.grid = grid;
        }

        #region A* pseudo
        /////////////////////// A* ///////////////////////
        ///add the starting node to the open list
        ///while the open list is not empty
        /// {
        ///  current node = node from open list with the lowest cost
        ///  if currentnode = goal
        ///    path complete
        ///  else
        ///    move current node to the closed list
        ///    for each adjacent node
        ///      if it lies with the field
        ///        and it isn't an obstacle
        ///          and it isn't on the open list
        ///            and isn't on the closed list
        ///              move it to the open list and calculate cost
        //////////////////////////////////////////////////
        #endregion

        public void SearchPath(TVec2 startTile, TVec2 endTile)
        {
            this.startTile = startTile;
            this.endTile = endTile;

            //Reset all the values
            for (int i = 0; i < AStartTest.gridWidth; i++)
            {
                for (int j = 0; j < AStartTest.gridHeight; j++)
                {
                    grid[i, j].cost = 0;
                    grid[i, j].heuristic = 0;
                }
            }

            #region Path validation
            bool canSearch = true;

            if (grid[(int)startTile.X, (int)startTile.Y].walkable == false)
            {
                Console.WriteLine("The start tile is non walkable. Choose a different value than: " + startTile.ToString());
                canSearch = false;
            }
            if (grid[(int)endTile.X, (int)endTile.Y].walkable == false)
            {
                Console.WriteLine("The end tile is non walkable. Choose a different value than: " + endTile.ToString());
                canSearch = false;
            }
            #endregion 

            //Start the A* algorithm
            if (canSearch)
            {
                //add the starting tile to the open list
                openList.Add(startTile);
                currentTile = new TVec2(-1, -1);

                //while Openlist is not empty
                while (openList.Count != 0)
                {
                    //current node = node from open list with the lowest cost
                    currentTile = GetTileWithLowestTotal(openList);

                    //If the currentTile is the endtile, then we can stop searching
                    if (currentTile.X == endTile.X && currentTile.Y == endTile.Y)
                    {
                        Console.WriteLine("YEHA, We found the end tile!!!! :D");
                        break;
                    }
                    else
                    {
                        //move the current tile to the closed list and remove it from the open list
                        openList.Remove(currentTile);
                        closedList.Add(currentTile);

                        //Get all the adjacent Tiles
                        List<TVec2> adjacentTiles = GetAdjacentTiles(currentTile);

                        foreach (TVec2 adjacentTile in adjacentTiles)
                        {
                            //adjacent tile can not be in the open list
                            if (!openList.Contains(adjacentTile))
                            {
                                //adjacent tile can not be in the closed list
                                if (!closedList.Contains(adjacentTile))
                                {
                                     //move it to the open list and calculate cost
                                    openList.Add(adjacentTile);

                                    Tile tile = grid[(int)adjacentTile.X, (int)adjacentTile.Y];

                                    //Calculate the cost
                                    tile.cost = grid[(int)currentTile.X, (int)currentTile.Y].cost + 1;

                                    //Calculate the manhattan distance
                                    tile.heuristic = ManhattanDistance(adjacentTile);

                                    //calculate the total amount
                                    tile.total = tile.cost + tile.heuristic;

                                    //make this tile green
                                    LE.EntityColor(tile.cube, Tile.GREEN);
                                }
                            }
                        }
                    }
                }
            }

            //Pain the start and end tile red
            LE.EntityColor(grid[(int)startTile.X, (int)startTile.Y].cube, Tile.RED);
            LE.EntityColor(grid[(int)endTile.X, (int)endTile.Y].cube, Tile.RED);

            //Show the path
            ShowPath();
        }

        public void ShowPath()
        {
            bool startFound = false;

            TVec2 currentTile = endTile;
            List<TVec2> pathTiles = new List<TVec2>();

            while (startFound == false)
            {
                List<TVec2> adjacentTiles = GetAdjacentTiles(currentTile);

                //check to see what newest current tile
                foreach (TVec2 adjacentTile in adjacentTiles)
                {
                    //Check if it is the start tile
                    if (adjacentTile.X == startTile.X && adjacentTile.Y == startTile.Y)
                        startFound = true;
                    //it has to be inside the closed as well as in the open list 
                    if (closedList.Contains(adjacentTile) || openList.Contains(adjacentTile))
                    {
                        if (grid[(int)adjacentTile.X, (int)adjacentTile.Y].cost <= grid[(int)currentTile.X, (int)currentTile.Y].cost
                            && grid[(int)adjacentTile.X, (int)adjacentTile.Y].cost > 0)
                        {
                            //Change the current Tile
                            currentTile = adjacentTile;

                            //Add this adjacent tile to the path list
                            pathTiles.Add(adjacentTile);

                            //Show the ball
                            LE.ShowEntity(grid[(int)adjacentTile.X, (int)adjacentTile.Y].ball);

                            break;
                        }
                    }
                }
            }
        }

        //Calculate the manhattan distance
        public int ManhattanDistance(TVec2 adjacentTile)
        {
            int manhattan = Math.Abs((int)(endTile.X - adjacentTile.X)) +  Math.Abs((int)(endTile.Y - adjacentTile.Y));
            return manhattan;
        }


        //Check if it is in the boundry and if it walkable
        public List<TVec2> GetAdjacentTiles(TVec2 currentTile)
        {
            List<TVec2> adjacentTiles = new List<TVec2>();
            TVec2 adjacentTile;

            //Tile above
            adjacentTile = new TVec2(currentTile.X, currentTile.Y + 1);
            if (adjacentTile.Y < AStartTest.gridHeight && grid[(int)adjacentTile.X, (int)adjacentTile.Y].walkable)
                adjacentTiles.Add(adjacentTile);

            //Tile underneath
            adjacentTile = new TVec2(currentTile.X, currentTile.Y - 1);
            if (adjacentTile.Y >= 0 && grid[(int)adjacentTile.X, (int)adjacentTile.Y].walkable)
                adjacentTiles.Add(adjacentTile);

            //Tile to the right
            adjacentTile = new TVec2(currentTile.X + 1, currentTile.Y);
            if (adjacentTile.X < AStartTest.gridWidth && grid[(int)adjacentTile.X, (int)adjacentTile.Y].walkable)
                adjacentTiles.Add(adjacentTile);

            //Tile to the left
            adjacentTile = new TVec2(currentTile.X - 1, currentTile.Y);
            if (adjacentTile.X >= 0 && grid[(int)adjacentTile.X, (int)adjacentTile.Y].walkable)
                adjacentTiles.Add(adjacentTile);

            //OPTIONAL DIAGONAL

            return adjacentTiles;
        }

        //Get the tile with the lowest total value
        public TVec2 GetTileWithLowestTotal(List<TVec2> openList)
        {
            //temp variables
            TVec2 tileWithLowestTotal = new TVec2(-1,-1);
            int lowestTotal = int.MaxValue;

            //search all the open tiles and get the tile with the lowest total cost
            foreach (TVec2 openTile in openList)
            {
                if (grid[(int)openTile.X, (int)openTile.Y].total <= lowestTotal)
                {
                    lowestTotal = grid[(int)openTile.X, (int)openTile.Y].total;
                    tileWithLowestTotal = new TVec2((int)openTile.X, (int)openTile.Y);
                }
            }

            return tileWithLowestTotal;
        }
    }
}
