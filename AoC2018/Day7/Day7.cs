using QuickGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2018.Day7
{
    public class Day7 : IDay
    {
        private Graph graph = new Graph();

        public Day7()
        {
            graph.ReadFromFile(Utilities.InputPath(7));
        }

        public string FirstPuzzle()
        {
            var res = graph.FindRoute();
            return res.Item1;
        }

        public string SecondPuzzle()
        {
            var res = graph.FindRoute(5);
            return res.Item2.ToString();
        }
    }

    public class Graph
    {
        private class EdgeComparer : IComparer<Edge<char>>
        {
            public int Compare(Edge<char> x, Edge<char> y)
            {
                return x.Target.CompareTo(y.Target);
            }
        }

        private class TraverseData
        {
            public char Root { get; set; }
            public SortedSet<Edge<char>> Discovered { get; set; } = new SortedSet<Edge<char>>(new EdgeComparer());
            public List<char> Visited { get; set; } = new List<char>();
            public List<Worker> Workers { get; set; } = new List<Worker>();
            public int TraverseTime { get; set; } = 0;
            public void Tick()
            {
                Workers.ForEach(w => w.Tick());
                TraverseTime += 1;
            }

            public void Visit(char vertex, Worker w)
            {
                if (vertex.Equals(GlobalRoot))
                {
                    w.OnFinished(vertex);
                }
                else
                {
                    w.Vertex = vertex;
                    w.TimeToFinish = WorkTime(vertex);
                }
            }

            public int WorkTime(char vertex)
            {
                return 61 + (vertex - 'A');
            }
        }

        private class Worker
        {
            public Worker(Finished onFinished)
            {
                OnFinished += onFinished;
            }

            public char Vertex { get; set; } = '.';
            public int TimeToFinish { get; set; } = 0;
            public bool IsIdle { get { return TimeToFinish == 0; } }
            public void Tick()
            {
                if (TimeToFinish > 1)
                {
                    TimeToFinish -= 1;
                }
                else if (TimeToFinish == 1)
                {
                    TimeToFinish -= 1;
                    OnFinished(Vertex);
                    Vertex = '.';
                }
            }
            public Finished OnFinished { get; set; }

            public delegate void Finished(char vertex);
        }

        private const char GlobalRoot = 'ρ';
        private const string edgePattern = @"Step (?<source>\w) must be finished before step (?<target>\w) can begin.";
        private static readonly Regex edgeRegex = new Regex(edgePattern);


        private BidirectionalGraph<char, Edge<char>> graph = new BidirectionalGraph<char, Edge<char>>();

        public void ReadFromFile(string path)
        {
            graph.Clear();
            using (var stream = new StreamReader(path))
            {
                while (!stream.EndOfStream)
                {
                    var e = ParseEdge(stream.ReadLine());
                    if (e != null)
                    {
                        AddEdge(e);
                    }
                }
            }
        }

        public Edge<char> ParseEdge(string line)
        {
            var m = edgeRegex.Match(line);
            if (!m.Success)
            {
                return null;
            }

            var source = m.Groups["source"].Value;
            var target = m.Groups["target"].Value;

            return new Edge<char>(source[0], target[0]);
        }

        public void AddEdge(Edge<char> e)
        {
            if (!graph.ContainsVertex(e.Source))
            {
                graph.AddVertex(e.Source);
            }

            if (!graph.ContainsVertex(e.Target))
            {
                graph.AddVertex(e.Target);
            }

            graph.AddEdge(e);
        }

        public ValueTuple<string, int> FindRoute(int numOfWorkers = 1)
        {
            var data = new TraverseData();
            ReduceToSingleRootCase();
            data.Root = GlobalRoot;
            Worker.Finished onFinished = vertex =>
            {
                data.Visited.Add(vertex);
                foreach (var e in graph.OutEdges(vertex))
                {
                    if (!data.Visited.Contains(e.Target))
                    {
                        data.Discovered.Add(e);
                    }
                }
            };

            for (int i = 0; i < numOfWorkers; i++)
            {
                data.Workers.Add(new Worker(onFinished));
            }


            VisitVertex(data.Root, data);
            var route = data.Visited.Skip(1).Aggregate("", (s, c) => s + c);
            var time = data.TraverseTime;

            return (route, time);
        }

        private void ReduceToSingleRootCase()
        {
            graph.AddVertex(GlobalRoot);
            foreach (var root in FindRoots())
            {
                graph.AddEdge(new Edge<char>(GlobalRoot, root));
            }
        }

        public string DrawGraph()
        {
            var alg = new QuickGraph.Graphviz.GraphvizAlgorithm<char, Edge<char>>(graph);
            alg.FormatVertex += (sender, e) => {
                e.VertexFormatter.Label = e.Vertex.ToString(); };
            return alg.Generate();
        }

        public List<char> FindRoots()
        {
            return graph.Vertices.Where(
                v => graph.InDegree(v) == 0).ToList();
        }

        private void VisitVertex(char vertex, TraverseData data)
        {
            var idleWorker = data.Workers.First(w => w.IsIdle);
            data.Visit(vertex, idleWorker);

            while (!data.Workers.Any(w => w.IsIdle))
            {
                data.Tick();
            }

            for(var edge = NextEdge(data); edge != null; )
            {
                var v = edge.Target;
                data.Discovered.Remove(edge);
                VisitVertex(v, data);
                edge = NextEdge(data);
                while (edge == null && !data.Workers.All(w => w.IsIdle))
                {
                    data.Tick();
                    edge = NextEdge(data);
                } 
            }
        }

        private Edge<char> NextEdge(TraverseData data)
        {
            foreach (var edge in data.Discovered)
            {
                var target = edge.Target;
                var inEdges = graph.InEdges(target);
                if (inEdges.All(e => data.Visited.Contains(e.Source)))
                {
                    return edge;
                }
            }

            return null;
        }

    }
}
