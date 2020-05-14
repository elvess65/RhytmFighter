﻿using Frameworks.Grid.Data;
using System;
using System.Collections.Generic;

public class FOVShadowcasting
{
    private int m_Width;
    private int m_Height;
    private SquareGrid m_Grid;

    private int m_PlayerX;
    private int m_PlayerY;

    private int VisualRange = 5;

    /// <summary>
    /// The octants which a player can see
    /// </summary>
    private int[] m_VisibleOctants = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

    /// <summary>
    /// List of points visible to the player
    /// </summary>
    public List<GridCellData> VisiblePoints { get; private set; }  // Cells the player can see

 
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
    public FOVShadowcasting(int width, int height, SquareGrid grid)
    {
        VisiblePoints = new List<GridCellData>();

        m_Width = width;
        m_Height = height;
        m_Grid = grid;
    }

    public void GetVisiblePonts(int playerX, int playerY)
    {
        m_PlayerX = playerX;
        m_PlayerY = playerY;

        foreach (int o in m_VisibleOctants)
            ScanOctant(1, o, 1.0, 0.0);
    }

    /// <summary>
    /// Examine the provided octant and calculate the visible cells within it.
    /// </summary>
    /// <param name="pDepth">Depth of the scan</param>
    /// <param name="pOctant">Octant being examined</param>
    /// <param name="pStartSlope">Start slope of the octant</param>
    /// <param name="pEndSlope">End slope of the octance</param>
    private void ScanOctant(int pDepth, int pOctant, double pStartSlope, double pEndSlope)
    {
        int visrange2 = VisualRange * VisualRange;
        int x = 0;
        int y = 0;

        switch (pOctant)
        {

            case 1: //nnw
                y = m_PlayerY - pDepth;
                if (y < 0) return;

                x = m_PlayerX - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0) x = 0;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {
                        if (CellIsNotWalkable(x, y)) //current cell blocked
                        {
                            if (x - 1 >= 0 && CellIsWalkable(x - 1, y)) //prior cell within range AND open...
                                                                        //...incremenet the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, m_PlayerX, m_PlayerY, false));
                        }
                        else
                        {

                            if (x - 1 >= 0 && CellIsNotWalkable(x - 1, y)) //prior cell within range AND open...
                                                                           //..adjust the startslope
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, m_PlayerX, m_PlayerY, false);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    x++;
                }
                x--;
                break;

            case 2: //nne

                y = m_PlayerY - pDepth;
                if (y < 0) return;

                x = m_PlayerX + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= m_Width) x = m_Width - 1;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {
                        if (CellIsNotWalkable(x, y))
                        {
                            if (x + 1 < m_Width && CellIsWalkable(x + 1, y))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, m_PlayerX, m_PlayerY, false));
                        }
                        else
                        {
                            if (x + 1 < m_Width && CellIsNotWalkable(x + 1, y))
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, m_PlayerX, m_PlayerY, false);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    x--;
                }
                x++;
                break;

            case 3:

                x = m_PlayerX + pDepth;
                if (x >= m_Width) return;

                y = m_PlayerY - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {

                        if (CellIsNotWalkable(x, y))
                        {
                            if (y - 1 >= 0 && CellIsWalkable(x, y - 1))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, m_PlayerX, m_PlayerY, true));
                        }
                        else
                        {
                            if (y - 1 >= 0 && CellIsNotWalkable(x, y - 1))
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, m_PlayerX, m_PlayerY, true);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    y++;
                }
                y--;
                break;

            case 4:

                x = m_PlayerX + pDepth;
                if (x >= m_Width) return;

                y = m_PlayerY + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= m_Height) y = m_Height - 1;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {

                        if (CellIsNotWalkable(x, y))
                        {
                            if (y + 1 < m_Height && CellIsWalkable(x, y + 1))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, m_PlayerX, m_PlayerY, true));
                        }
                        else
                        {
                            if (y + 1 < m_Height && CellIsNotWalkable(x, y + 1))
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, m_PlayerX, m_PlayerY, true);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    y--;
                }
                y++;
                break;

            case 5:

                y = m_PlayerY + pDepth;
                if (y >= m_Height) return;

                x = m_PlayerX + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= m_Width) x = m_Width - 1;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {

                        if (CellIsNotWalkable(x, y))
                        {
                            if (x + 1 < m_Height && CellIsWalkable(x + 1, y))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, m_PlayerX, m_PlayerY, false));
                        }
                        else
                        {
                            if (x + 1 < m_Height && CellIsNotWalkable(x + 1, y))
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, m_PlayerX, m_PlayerY, false);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    x--;
                }
                x++;
                break;

            case 6:

                y = m_PlayerY + pDepth;
                if (y >= m_Height) return;

                x = m_PlayerX - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0) x = 0;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {
                        if (CellIsNotWalkable(x, y))
                        {
                            if (x - 1 >= 0 && CellIsWalkable(x - 1, y))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, m_PlayerX, m_PlayerY, false));
                        }
                        else
                        {
                            if (x - 1 >= 0 && CellIsNotWalkable(x - 1, y))
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, m_PlayerX, m_PlayerY, false);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    x++;
                }
                x--;
                break;

            case 7:

                x = m_PlayerX - pDepth;
                if (x < 0) return;

                y = m_PlayerY + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= m_Height) y = m_Height - 1;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {

                        if (CellIsNotWalkable(x, y))
                        {
                            if (y + 1 < m_Height && CellIsWalkable(x, y + 1))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, m_PlayerX, m_PlayerY, true));
                        }
                        else
                        {
                            if (y + 1 < m_Height && CellIsNotWalkable(x, y + 1))
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, m_PlayerX, m_PlayerY, true);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    y--;
                }
                y++;
                break;

            case 8: //wnw

                x = m_PlayerX - pDepth;
                if (x < 0) return;

                y = m_PlayerY - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, m_PlayerX, m_PlayerY, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, m_PlayerX, m_PlayerY) <= visrange2)
                    {

                        if (CellIsNotWalkable(x, y))
                        {
                            if (y - 1 >= 0 && CellIsWalkable(x, y - 1))
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, m_PlayerX, m_PlayerY, true));

                        }
                        else
                        {
                            if (y - 1 >= 0 && CellIsNotWalkable(x, y - 1))
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, m_PlayerX, m_PlayerY, true);

                            VisiblePoints.Add(m_Grid.GetCellByCoord(x, y));
                        }
                    }
                    y++;
                }
                y--;
                break;
        }


        if (x < 0)
            x = 0;
        else if (x >= m_Width)
            x = m_Width - 1;

        if (y < 0)
            y = 0;
        else if (y >= m_Height)
            y = m_Height - 1;

        if (pDepth < VisualRange & CellIsWalkable(x, y))
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

    private bool CellIsNotWalkable(int x, int y)
    {
        GridCellData cell = m_Grid.GetCellByCoord(x, y);
        return cell == null || cell.CellType != CellTypes.Normal || cell.HasObject;
    }

    private bool CellIsWalkable(int x, int y)
    {
        return !CellIsNotWalkable(x, y);
    }
}
