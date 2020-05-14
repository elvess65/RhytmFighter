using Frameworks.Grid.Data;
using System;
using System.Collections.Generic;

public class FOVShadowcasting
{
    public int Width;
    public int Height;
    SquareGrid Grid;

    public int PlayerX;
    public int PlayerY;

    /// <summary>
    /// List of points visible to the player
    /// </summary>
    public List<GridCellData> VisiblePoints { get; private set; }  // Cells the player can see

    /// <summary>
    /// The octants which a player can see
    /// </summary>
    private List<int> VisibleOctants = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

    public int VisualRange = 5;

    //  Octant data
    //
    //    \ 1 | 2 /
    //   8 \  |  / 3
    //   -----+-----
    //   7 /  |  \ 4
    //    / 6 | 5 \
    //
    //  1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW

    /// <summary>
    /// Start here: go through all the octants which surround the player to
    /// determine which open cells are visible
    /// </summary>
    public FOVShadowcasting(int w, int h, SquareGrid grid)
    {
        VisiblePoints = new List<GridCellData>();
        Width = w;
        Height = h;
        Grid = grid;
    }

    public void GetVisible(int playerX, int playerY)
    {
        PlayerX = playerX;
        PlayerY = playerY;

        foreach (int o in VisibleOctants)
            ScanOctant(1, o, 1.0, 0.0);
    }

    /// <summary>
    /// Examine the provided octant and calculate the visible cells within it.
    /// </summary>
    /// <param name="pDepth">Depth of the scan</param>
    /// <param name="pOctant">Octant being examined</param>
    /// <param name="pStartSlope">Start slope of the octant</param>
    /// <param name="pEndSlope">End slope of the octance</param>
    protected void ScanOctant(int pDepth, int pOctant, double pStartSlope, double pEndSlope)
    {
        int visrange2 = VisualRange * VisualRange;
        int x = 0;
        int y = 0;

        switch (pOctant)
        {

            case 1: //nnw
                y = PlayerY - pDepth;
                if (y < 0) return;

                x = PlayerX - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0) x = 0;

                while (GetSlope(x, y, PlayerX, PlayerY, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {
                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y))) //current cell blocked
                        {
                            if (x - 1 >= 0 && Grid.CellIsWalkable(Grid.GetCellByCoord(x - 1, y))) //prior cell within range AND open...
                                                                  //...incremenet the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, PlayerX, PlayerY, false));
                        }
                        else
                        {

                            if (x - 1 >= 0 && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x - 1, y))) //prior cell within range AND open...
                                                                  //..adjust the startslope
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, PlayerX, PlayerY, false);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    x++;
                }
                x--;
                break;

            case 2: //nne

                y = PlayerY - pDepth;
                if (y < 0) return;

                x = PlayerX + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= Width) x = Width - 1;

                while (GetSlope(x, y, PlayerX, PlayerY, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {
                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (x + 1 < Width && Grid.CellIsWalkable(Grid.GetCellByCoord(x + 1, y)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, PlayerX, PlayerY, false));
                        }
                        else
                        {
                            if (x + 1 < Width && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x + 1, y)))
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, PlayerX, PlayerY, false);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    x--;
                }
                x++;
                break;

            case 3:

                x = PlayerX + pDepth;
                if (x >= Width) return;

                y = PlayerY - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, PlayerX, PlayerY, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {

                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (y - 1 >= 0 && Grid.CellIsWalkable(Grid.GetCellByCoord(x, y - 1)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, PlayerX, PlayerY, true));
                        }
                        else
                        {
                            if (y - 1 >= 0 && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y - 1)))
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, PlayerX, PlayerY, true);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    y++;
                }
                y--;
                break;

            case 4:

                x = PlayerX + pDepth;
                if (x >= Width) return;

                y = PlayerY + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= Height) y = Height - 1;

                while (GetSlope(x, y, PlayerX, PlayerY, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {

                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (y + 1 < Height && Grid.CellIsWalkable(Grid.GetCellByCoord(x, y + 1)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, PlayerX, PlayerY, true));
                        }
                        else
                        {
                            if (y + 1 < Height && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y + 1)))
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, PlayerX, PlayerY, true);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    y--;
                }
                y++;
                break;

            case 5:

                y = PlayerY + pDepth;
                if (y >= Height) return;

                x = PlayerX + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= Width) x = Width - 1;

                while (GetSlope(x, y, PlayerX, PlayerY, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {

                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (x + 1 < Height && Grid.CellIsWalkable(Grid.GetCellByCoord(x + 1, y)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, PlayerX, PlayerY, false));
                        }
                        else
                        {
                            if (x + 1 < Height && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x + 1, y)))
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, PlayerX, PlayerY, false);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    x--;
                }
                x++;
                break;

            case 6:

                y = PlayerY + pDepth;
                if (y >= Height) return;

                x = PlayerX - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0) x = 0;

                while (GetSlope(x, y, PlayerX, PlayerY, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {
                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (x - 1 >= 0 && Grid.CellIsWalkable(Grid.GetCellByCoord(x - 1, y)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, PlayerX, PlayerY, false));
                        }
                        else
                        {
                            if (x - 1 >= 0 && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x - 1, y)))
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, PlayerX, PlayerY, false);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    x++;
                }
                x--;
                break;

            case 7:

                x = PlayerX - pDepth;
                if (x < 0) return;

                y = PlayerY + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= Height) y = Height - 1;

                while (GetSlope(x, y, PlayerX, PlayerY, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {

                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (y + 1 < Height && Grid.CellIsWalkable(Grid.GetCellByCoord(x, y + 1)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, PlayerX, PlayerY, true));
                        }
                        else
                        {
                            if (y + 1 < Height && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y + 1)))
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, PlayerX, PlayerY, true);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    y--;
                }
                y++;
                break;

            case 8: //wnw

                x = PlayerX - pDepth;
                if (x < 0) return;

                y = PlayerY - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, PlayerX, PlayerY, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, PlayerX, PlayerY) <= visrange2)
                    {

                        if (Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y)))
                        {
                            if (y - 1 >= 0 && Grid.CellIsWalkable(Grid.GetCellByCoord(x, y - 1)))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, PlayerX, PlayerY, true));

                        }
                        else
                        {
                            if (y - 1 >= 0 && Grid.CellIsNotWalkable(Grid.GetCellByCoord(x, y - 1)))
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, PlayerX, PlayerY, true);

                            VisiblePoints.Add(Grid.GetCellByCoord(x, y));
                        }
                    }
                    y++;
                }
                y--;
                break;
        }


        if (x < 0)
            x = 0;
        else if (x >= Width)
            x = Width - 1;

        if (y < 0)
            y = 0;
        else if (y >= Height)
            y = Height - 1;

        if (pDepth < VisualRange & Grid.CellIsWalkable(Grid.GetCellByCoord(x, y)))
            ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope);

    }

    /// <summary>
    /// Get the gradient of the slope formed by the two points
    /// </summary>
    /// <param name="pX1"></param>
    /// <param name="pY1"></param>
    /// <param name="pX2"></param>
    /// <param name="pY2"></param>
    /// <param name="pInvert">Invert slope</param>
    /// <returns></returns>
    private double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
    {
        if (pInvert)
            return (pY1 - pY2) / (pX1 - pX2);
        else
            return (pX1 - pX2) / (pY1 - pY2);
    }


    /// <summary>
    /// Calculate the distance between the two points
    /// </summary>
    /// <param name="pX1"></param>
    /// <param name="pY1"></param>
    /// <param name="pX2"></param>
    /// <param name="pY2"></param>
    /// <returns>Distance</returns>
    private int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
    {
        return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
    }
}
