using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018.Day8
{
    public class Day8 : IDay
    {
        private Node root;

        public Day8()
        {
            root = Node.ReadFromFile(Utilities.InputPath(8));
        }

        public string FirstPuzzle()
        {
            return root.MetadataSum().ToString();
        }

        public string SecondPuzzle()
        {
            return root.Value().ToString();
        }
    }

    public class Node
    {
        public int ChildrenCount { get { return Children.Count; } }
        public int MetadataCount { get { return Metadata.Count; } }
        public List<Node> Children { get; private set; } = new List<Node>();
        public List<int> Metadata { get; private set; } = new List<int>();

        public static Node ReadFromFile(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var tree = reader.ReadToEnd()
                                 .Trim()
                                 .Split()
                                 .Select(token => int.Parse(token))
                                 .ToList<int>();
                return new Node(tree.GetEnumerator());
            }
        }

        public Node(IEnumerator<int> enumerator)
        {
            enumerator.MoveNext();
            var childrenCount = enumerator.Current;
            enumerator.MoveNext();
            var metadataCount = enumerator.Current;


            for (int i = 0; i < childrenCount; i++)
            {
                Children.Add(new Node(enumerator));
            }

            for (int i = 0; i < metadataCount; i++)
            {
                enumerator.MoveNext();
                Metadata.Add(enumerator.Current);
            }
        }

        public int MetadataSum()
        {
            var sum = Metadata.Sum();
            foreach (var child in Children)
            {
                sum += child.MetadataSum();
            }

            return sum;
        }

        public int Value()
        {
            if (ChildrenCount == 0)
            {
                return Metadata.Sum();
            }
            else
            {
                return Metadata
                    .Select(m => m - 1)
                    .Where(m => m >= 0 && m < ChildrenCount)
                    .Select(idx => Children[idx].Value())
                    .Sum();
            }
        }
    }
}
