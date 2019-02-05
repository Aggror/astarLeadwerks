using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeadwerksEngine;

namespace AStartTest
{
    public class Tile
    {
        public TVec2 ID;
        public TMesh cube;
        public TMesh ball;
        public int cost = 0;
        public int heuristic = 0;
        public int total = 0;
        public bool walkable;

        public static TVec4 RED = new TVec4(255,0,0,0);
        public static TVec4 GREEN = new TVec4(0, 255, 0, 0);
        public static TVec4 BLACK = new TVec4(0, 0, 0, 1);
        public static TVec4 PINK = new TVec4(60, 60, 60, 0);
        public static TVec4 YELLOW = new TVec4(255, 255, 0, 0);

        public Tile(TVec2 ID, TVec3 scale, TVec3 position, TMesh cube, TMesh ball, bool walkable)
        {
            this.ID = ID;
            this.cube = LE.CopyEntity(cube);
            this.ball = LE.CopyEntity(ball);
            this.walkable = walkable;
            LE.ShowEntity(this.cube);

            //set position and scale
            LE.ScaleEntity(this.cube, scale);
            LE.PositionEntity(this.cube, position);

            LE.ScaleEntity(this.ball, new TVec3(scale.X / 2, scale.Y / 2, scale.Z / 2));
            LE.PositionEntity(this.ball, new TVec3(position.X, position.Y + 1, position.Z));

            //Set a different Color when the tile is non walkable
            if (!this.walkable)
                LE.EntityColor(this.cube, Tile.BLACK);
            else
            {
                LE.EntityColor(this.cube, Tile.PINK);
                LE.EntityColor(this.ball, Tile.YELLOW);
            }
        }
    }
}
