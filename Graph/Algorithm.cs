using System;
using System.Collections.Generic;
using System.IO;

namespace Graph
{
    public class Algorithm
    {
        private SortedDictionary<string, SortedSet<string>> edges;

        private int colors, n, m;

        private SortedDictionary<string, int> color;

        private void DFS(ref List<string> order, string u, int col)
        {
            color.Add(u, col);

            foreach (var v in edges[u])
                if (!color.ContainsKey(v)) DFS(ref order, v, col);

            order.Add(u);
        }

        public Algorithm(WorkGraph g)
        {
            edges = g.Edges; n = g.N; m = g.M;
            color = new SortedDictionary<string, int>();
            List<string> order = new List<string>();

            foreach (var p in edges)
            {
                string u = p.Key;

                if (!color.ContainsKey(u)) DFS(ref order, u, 1);
            }

            order.Reverse();
            color.Clear();

            int col = 0;

            SortedDictionary<string, SortedSet<string>> inv_edges = new SortedDictionary<string, SortedSet<string>>();

            foreach (var p in edges)
            {
                string u = p.Key;
                inv_edges.Add(u, new SortedSet<string>());
            }

            foreach (var p in edges)
            {
                string u = p.Key;

                foreach (var v in edges[u])
                    inv_edges[v].Add(u);
            }

            SortedDictionary<string, SortedSet<string>> copy_edges = edges;
            edges = inv_edges;

            foreach (var u in order)
                if (!color.ContainsKey(u))
                {
                    List<string> component = new List<string>();

                    DFS(ref component, u, ++col);
                }

            colors = col;
            edges = copy_edges;
        }

        public void Print(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("CLASTERS: {0}", colors);
                file.WriteLine("VERTICES: {0}", n);

                foreach (var p in edges)
                    file.WriteLine("{0} {1}", p.Key, color[p.Key]);

                file.WriteLine("EDGES: {0}", m);

                foreach (var p in edges)
                {
                    string u = p.Key;

                    foreach (var v in edges[u])
                        file.WriteLine("{0} {1}", u, v);
                }
            }
        }
    }
}
