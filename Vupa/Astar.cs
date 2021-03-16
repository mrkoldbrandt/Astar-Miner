﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vupa
{
    public class AStar
    {
        public List<Node> closed;

        public List<Node> open;
        public static Node neighbour;

        public List<Node> Closed
        {
            get { return closed; }
            set { closed = value; }
        }

        public List<Node> Open
        {
            get { return open; }
            set { open = value; }
        }

        public AStar()
        {
            closed = new List<Node>();
            open = new List<Node>();
        }


        //creates a list of nodes to put into the final path.
        //sets the starting node as current node to have a position to evaluate other nodes from. 
        //runs a while loop with nested for loops in, to check the x,y positions of ajacent nodes
        //runs through the nodes around currentnode and gives them a G value of 10 if they are (up, down, left, right) or a G value of 14 if they are diagonal to currentnode
        //

        public List<Node> FindPath(Point myStart, Point myGoal, List<Node> nodes)
        {
            Point start = myStart;
            Point goal = myGoal;
            closed.Clear();
            open.Clear();
            List<Node> finalPath = new List<Node>();
           

            Node currentNode = nodes.Find(x => x.Position == start);

            open.Add(currentNode);

            while (true)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        if (x != 0 || y != 0)
                        {
                            neighbour = nodes.Find(node => node.Position.X == currentNode.Position.X - x && node.Position.Y == currentNode.Position.Y - y);

                            if (neighbour != null)
                            {
                                int gCost = 0;

                                if (Math.Abs(x - y) % 2 == 1)
                                {
                                    gCost = 10;
                                }
                                else
                                {
                                    gCost = 14;
                                }

                                if (open.Exists(n => n == neighbour))
                                {
                                    if (currentNode.G + gCost < neighbour.G)
                                    {
                                        neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                    }

                                }
                                else if (!closed.Exists(n => n == neighbour))
                                {

                                    if (gCost == 14)
                                    {
                                        if (currentNode.Position.X < neighbour.Position.X && currentNode.Position.Y > neighbour.Position.Y) //Topright
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y - 1) && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X + 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbour);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X > neighbour.Position.X && currentNode.Position.Y < neighbour.Position.Y) //Bottomleft
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y + 1) && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X - 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbour);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X > neighbour.Position.X && currentNode.Position.Y > neighbour.Position.Y) //Topleft
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y - 1) && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X - 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbour);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X < neighbour.Position.X && currentNode.Position.Y < neighbour.Position.Y) //Bottomright
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X + 1, currentNode.Position.Y) && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X, currentNode.Position.Y + 1))))
                                            {
                                                open.Add(neighbour);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        open.Add(neighbour);
                                        open.Remove(currentNode);
                                        closed.Add(currentNode);
                                        neighbour.CalcValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                    }
                                }
                            }

                        }
                    }
                }

                if (open.Count == 0)
                {
                    break;
                }

                open.Sort();

                currentNode = open[0];
                open.Remove(currentNode);
                closed.Add(currentNode);

                if (currentNode.Position == goal)
                {
                    closed.Add(currentNode);
                    break;
                }
            }

            Node tmpNode = closed.Find(x => x.Position == goal);

            if (tmpNode != null)
            {
                while (!finalPath.Exists(x => x.Position == start))
                {
                    finalPath.Add(tmpNode);
                    tmpNode = tmpNode.Parent;
                }
            }

            return finalPath;
        }
    }
}
