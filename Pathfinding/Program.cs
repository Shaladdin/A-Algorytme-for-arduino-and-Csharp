using System;

namespace Pathfinding
{
    struct Vector2
    {
        public int x, y; public Vector2(int X, int Y) { x = X; y = Y; } //this is the important part
        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.x + b.x, a.y + b.y); }
        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.x - b.x, a.y - b.y); }
        public static Vector2 operator *(Vector2 a, int b) { return new Vector2(a.x * b, a.y * b); }
        public static Vector2 operator /(Vector2 a, int b) { if (a != 0 && b != 0) return new Vector2(a.x / b, a.y / b); else throw new DivideByZeroException(); }
        public static bool operator ==(Vector2 a, Vector2 b) { return (a.x == b.x) && (a.y == b.y); }
        public static bool operator !=(Vector2 a, Vector2 b) { return !(a == b); }
        public static bool operator >(Vector2 a, Vector2 b) { return a.x > b.x && a.y > b.y; }
        public static bool operator <(Vector2 a, Vector2 b) { return a.x < b.x && a.y < b.y; }
        public static bool operator ==(Vector2 a, int b) { return a.x == b && a.y == b; }
        public static bool operator !=(Vector2 a, int b) { return !(a == b); }
        public static bool operator >=(Vector2 a, int b) { return a.x >= b && a.y >= b; }
        public static bool operator <=(Vector2 a, int b) { return a.x <= b && a.y <= b; }
        public static bool operator >(Vector2 a, int b) { return a.x > b && a.y > b; }
        public static bool operator <(Vector2 a, int b) { return a.x < b && a.y < b; }

        //whatafer this is, this trash is useles and anoying↓↓↓
        public override int GetHashCode()
        {
            return x;
        }
        public override bool Equals(object o)
        {
            if (o != null) return false;
            else return x == y;
        }

    }
    struct Node
    {
        public enum type { start, end, normal, wall, empety }
        public type nodeType { get; set; }
        public bool evaluated { get; set; }
        public Vector2 position { get; set; }
        public Vector2 parentDirection { get; set; }
        public int Gcost { get; set; }
        public int Hcost { get; set; }

        public int Fcost { get { return Gcost + Hcost; } }
        public Node(int gcost, int hcost, Vector2 Position, Vector2 ParentDirection, type Type)
        {
            Gcost = gcost;
            Hcost = hcost;
            evaluated = false;
            parentDirection = ParentDirection;
            nodeType = Type;
            position = Position;
        }
    }


    class Program
    {
        static int difference(int num1, int num2)
        {
            return Math.Max(num1, num2) - Math.Min(num1, num2);
        }
        static int[,] map =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 3, 2, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 3, 3, 3, 3, 3, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
        static void Main(string[] args)
        {
            bool done = false;
            Vector2 mapLength = new Vector2(map.GetLength(1), map.GetLength(0));
            Vector2 startPosition = new Vector2();
            Vector2 endPosition = new Vector2();
            Node[,] nodes = new Node[mapLength.y, mapLength.x];
            // int y = ary.GetLength(0);
            // int x = ary.GetLength(1);
            bool startExist = false;
            bool endExist = false;
            for (int y = 0; y < mapLength.y; y++)
            {
                for (int x = 0; x < mapLength.x; x++)
                {
                    switch (map[y, x])
                    {
                        case 0:
                            nodes[y, x] = new Node(0, 0, new Vector2(x, y), Vector2.Zero, Node.type.empety);
                            break;
                        case 1:
                            if (!startExist)
                            {
                                nodes[y, x] = new Node(0, 0, new Vector2(x, y), new Vector2(), Node.type.start);
                                startPosition = new Vector2(x, y);
                                startExist = true;
                            }
                            else { throw new ArgumentException("There Cant be more than one start point in one map!"); }
                            break;
                        case 2:
                            if (!endExist)
                            {
                                nodes[y, x] = new Node(0, 0, new Vector2(x, y), new Vector2(), Node.type.end);
                                endPosition = new Vector2(x, y);
                                endExist = true;
                            }
                            else
                            { throw new ArgumentException("There Cant be more than one end point in one map"); }
                            break;
                        case 3:
                            nodes[y, x] = new Node(0, 0, new Vector2(x, y), Vector2.Zero, Node.type.wall);
                            break;
                    }
                }
            }

            int getCost(Vector2 node, Vector2 pos)
            {
                int x = difference(node.x, pos.x);
                int y = difference(node.y, pos.y);
                int bigNum = Math.Max(x, y);
                int smollNum = bigNum == x ? y : x;
                return smollNum * 14 + bigNum - smollNum;
            }
            int getGCost(Vector2 node)
            { return getCost(node, startPosition); }
            int getHCost(Vector2 node)
            { return getCost(node, endPosition); }
            int calculateGCost(Node node)
            {
                Vector2 parent = node.position + node.parentDirection;
                int move = node.parentDirection.x == 0 || node.parentDirection.y == 0 ? 10 : 14;
                return nodes[parent.y, parent.x].Gcost + move;
            }

            void evaluate(Vector2 node)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector2 pos = new Vector2(node.x + x, node.y + y) * -1;
                        bool legal = !(pos.x == 0 && pos.y == 0) && pos >= 0 && pos < mapLength;
                        if (legal)
                        {
                            switch (nodes[pos.y, pos.x].nodeType)
                            {
                                case Node.type.empety:
                                    nodes[pos.y, pos.x] = new Node(0, getHCost(pos), pos, new Vector2(x, y) * -1, Node.type.normal);
                                    nodes[pos.y, pos.x].Gcost = calculateGCost(nodes[pos.y, pos.x]);
                                    break;
                                case Node.type.normal:
                                    if (nodes[pos.y, pos.x].evaluated)
                                    {
                                        Node g = new Node(0, getHCost(pos), pos, new Vector2(x, y) * -1, Node.type.normal);
                                        int j = calculateGCost(g);
                                        g.Gcost = j;
                                        int i = Math.Min(nodes[pos.y, pos.x].Fcost, j);
                                        if (i != nodes[pos.y, pos.x].Fcost)
                                            nodes[pos.y, pos.x] = g;
                                    }
                                    else
                                        Console.WriteLine("bruh");
                                    break;
                                case Node.type.end:
                                    done = true;
                                    nodes[pos.y, pos.x].parentDirection = new Vector2(x, y);
                                    break;
                            }
                        }
                    }
                }
            }
            if (!startExist || !endExist)
            { throw new System.ArgumentException("Map is incomplete, there must be one start point and end points!"); }
            else
            {
                bool somthingEvaluated = false;
                Vector2 target = new Vector2();
                nodes[startPosition.y, startPosition.x].Hcost = getHCost(nodes[startPosition.y, startPosition.x].position);
                nodes[endPosition.y, endPosition.x].Gcost = getGCost(nodes[endPosition.y, endPosition.x].position);
                int smallestFcost = nodes[startPosition.y, startPosition.x].Fcost;
                int smallestHcost = nodes[startPosition.y, startPosition.x].Hcost;
                int smallestGcost = nodes[endPosition.y, endPosition.x].Gcost;
                while (!done)
                {

                    //find smallest Fcost
                    for (int x = 0; x < mapLength.x; x++)
                    {
                        for (int y = 0; y < mapLength.y; y++)
                        {
                            if ((nodes[y, x].nodeType == Node.type.normal || nodes[y, x].nodeType == Node.type.start) && nodes[y, x].evaluated == false)
                            {
                                smallestFcost = Math.Min(nodes[y, x].Fcost, smallestFcost);
                                if (nodes[y, x].Fcost == smallestFcost)
                                    smallestHcost = Math.Min(nodes[y, x].Hcost, smallestHcost);
                                if (nodes[y, x].Hcost == smallestHcost && nodes[y, x].nodeType != Node.type.start)
                                    smallestGcost = Math.Min(nodes[y, x].Gcost, smallestGcost);
                            }
                        }
                    }
                    for (int x = 0; x < mapLength.x; x++)
                    {
                        for (int y = 0; y < mapLength.y; y++)
                        {
                            if ((nodes[y, x].nodeType == Node.type.normal || nodes[y, x].nodeType == Node.type.start) && nodes[y, x].Fcost == smallestFcost && nodes[y, x].Gcost == smallestGcost)
                            {
                                target = nodes[y, x].position;
                                if (nodes[y, x].Hcost == smallestHcost)
                                {
                                    somthingEvaluated = true;
                                    evaluate(nodes[y, x].position);
                                }
                            }
                        }
                    }
                    if (!somthingEvaluated)
                        evaluate(target);
                }
                Console.WriteLine("Done");
            }
        }
    }
}
