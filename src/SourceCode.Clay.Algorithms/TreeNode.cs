#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SourceCode.Clay.Algorithms
{
    [DebuggerDisplay("{HierarchyPath,nq} {Node}")]
    public readonly struct TreeNode<T> : IEquatable<TreeNode<T>>, IStructuralEquatable
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        public IReadOnlyList<int> Hierarchy { get; }

        public T Node { get; }

        public string HierarchyPath
        {
            get
            {
                if (Hierarchy == null) return null;

                var sb = new StringBuilder(1 + Hierarchy.Count * 2);
                sb.Append("/");
                for (var i = 0; i < Hierarchy.Count; i++)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}/", Hierarchy[i]);

                return sb.ToString();
            }
        }

        public TreeNode(T node, IEqualityComparer<T> equalityComparer, IReadOnlyList<int> hierarchy)
        {
            Hierarchy = hierarchy ?? throw new ArgumentNullException(nameof(hierarchy));
            Node = node;
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public TreeNode(T node, IEqualityComparer<T> equalityComparer, params int[] hierarchy)
            : this(node, equalityComparer, (IReadOnlyList<int>)hierarchy)
        { }

        public TreeNode(T node, IReadOnlyList<int> hierarchy)
            : this(node, null, hierarchy)
        { }

        public TreeNode(T node, params int[] hierarchy)
            : this(node, null, (IReadOnlyList<int>)hierarchy)
        { }

        public TreeNode<T> Clone() 
            => Hierarchy is null ? 
            default : 
            new TreeNode<T>(Node, _equalityComparer, Hierarchy.ToArray()); // Clone via ToArray

        public override string ToString() => $"{HierarchyPath} {Node}";

        public override bool Equals(object obj)
            => obj is TreeNode<T> other
            && Equals(other);

        public bool Equals(TreeNode<T> other)
        {
            if (Hierarchy is null) return other.Hierarchy is null;
            if (other.Hierarchy is null) return false;

            if (Hierarchy.Count != other.Hierarchy.Count) return false;
            if (!_equalityComparer.Equals(Node, other.Node)) return false;

            for (var i = 0; i < Hierarchy.Count; i++)
            {
                if (Hierarchy[i] != other.Hierarchy[i]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1781160927;

            hashCode = hashCode * -1521134295 + _equalityComparer.GetHashCode(Node);

            if (Hierarchy != null)
            {
                hashCode = hashCode * -1521134295 + Hierarchy.Count;
                var step = Math.Max(1, Hierarchy.Count / 10);
                for (var i = Hierarchy.Count - 1; i >= 0; i -= step)
                {
                    hashCode = hashCode * -1521134295 + Hierarchy[i].GetHashCode();
                }
            }

            return hashCode;
        }

        public static bool operator ==(TreeNode<T> node1, TreeNode<T> node2) 
            => node1.Equals(node2);

        public static bool operator !=(TreeNode<T> node1, TreeNode<T> node2) 
            => !(node1 == node2);

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(Node);
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(Hierarchy);
            return hashCode;
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
            => other is TreeNode<T> e
            && (comparer ?? throw new ArgumentNullException(nameof(comparer))).Equals(Node, e.Node)
            && comparer.Equals(Hierarchy, e.Hierarchy);
    }
}
