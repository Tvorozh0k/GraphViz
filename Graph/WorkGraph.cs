using System;
using System.Collections.Generic;
using System.IO;

namespace Graph
{
    public class WorkGraph
    {
        private int _N = 0, _M = 0;

        private SortedDictionary<string, SortedSet<string>> graph = new SortedDictionary<string, SortedSet<string>>();

        public WorkGraph() { }

        public WorkGraph(string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                var line = file.ReadLine().Split();

                line = file.ReadLine().Split();

                _N = int.Parse(line[1]);

                for (int i = 0; i < _N; ++i)
                {
                    string v = file.ReadLine();
                    graph.Add(v, new SortedSet<string>());
                }

                line = file.ReadLine().Split();

                _M = int.Parse(line[1]);

                for (int i = 0; i < _M; ++i)
                {
                    line = file.ReadLine().Split();

                    string u = line[0], v = line[1];

                    graph[u].Add(v);
                }
            }
        }

        public int N { get { return _N; } }
        public int M { get { return _M; } }

        public SortedDictionary<string, SortedSet<string>> Edges { get { return graph; } }

        public int AddVertice(string v)
        {
            if (graph.ContainsKey(v)) return 1;
            else
            {
                ++_N;
                graph.Add(v, new SortedSet<string>());

                return 0;
            }
        }

        public int RemoveVertice(string v)
        {
            if (!graph.ContainsKey(v)) return 1;
            else
            {
                --_N; _M -= graph[v].Count;

                graph.Remove(v);

                foreach (var p in graph)
                    if (graph[p.Key].Contains(v))
                    {
                        --_M;
                        graph[p.Key].Remove(v);
                    }

                return 0;
            }
        }

        public int AddEdge(string u, string v)
        {
            if (!graph.ContainsKey(u)) return 1;
            else if (!graph.ContainsKey(v)) return 2;
            else if (graph[u].Contains(v)) return 3;
            else
            {
                ++_M;
                graph[u].Add(v);

                return 0;
            }
        }

        public int RemoveEdge(string u, string v)
        {
            if (!graph.ContainsKey(u)) return 1;
            else if (!graph.ContainsKey(v)) return 2;
            else if (!graph[u].Contains(v)) return 3;
            else
            {
                --_M;
                graph[u].Remove(v);

                return 0;
            }
        }

        public void clear()
        {
            _N = 0; _M = 0;
            graph.Clear();
        }

        public void Print(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("GRAPH directed unweighted");
                file.WriteLine("VERTICES: {0}", _N);

                foreach (var p in graph)
                    file.WriteLine(p.Key);

                file.WriteLine("EDGES: {0}", _M);

                foreach (var p in graph)
                {
                    string u = p.Key;

                    foreach (var v in graph[u])
                        file.WriteLine("{0} {1}", u, v);
                }
            }
        }
    }
}
