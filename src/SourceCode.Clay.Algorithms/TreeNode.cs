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
    public struct TreeNode<T> : IEquatable<TreeNode<T>>, IStructuralEquatable
    {
        #region Properties

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

        #endregion

        #region Constructors

        public TreeNode(T node, IReadOnlyList<int> hierarchy)
        {
            Hierarchy = hierarchy ?? throw new ArgumentNullException(nameof(hierarchy));
            Node = node;
        }

        public TreeNode(T node, params int[] hierarchy)
            : this(node, (IReadOnlyList<int>)hierarchy)
        {
        }

        #endregion

        #region Methods

        public static bool operator ==(TreeNode<T> node1, TreeNode<T> node2) => node1.Equals(node2);

        public static bool operator !=(TreeNode<T> node1, TreeNode<T> node2) => !(node1 == node2);

        public TreeNode<T> Clone()
        {
            if (Hierarchy is null) return default;
            return new TreeNode<T>(Node, Hierarchy.ToArray());
        }

        public override string ToString() => $"{HierarchyPath} {Node}";

        public override bool Equals(object obj)
            => obj is TreeNode<T> o && Equals(o);

        public bool Equals(TreeNode<T> other)
        {
            if (Hierarchy is null) return other.Hierarchy is null;
            if (other.Hierarchy is null) return false;

            if (!EqualityComparer<T>.Default.Equals(Node, other.Node)) return false;
            if (Hierarchy.Count != other.Hierarchy.Count) return false;

            for (var i = Hierarchy.Count - 1; i >= 0; i--)
            {
                if (Hierarchy[i] != other.Hierarchy[i]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1781160927;

            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Node);

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

        public int GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(Node);
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(Hierarchy);
            return hashCode;
        }

        public bool Equals(object other, IEqualityComparer comparer)
            => other is TreeNode<T> e
            && (comparer ?? throw new ArgumentNullException(nameof(comparer))).Equals(Node, e.Node)
            && comparer.Equals(Hierarchy, e.Hierarchy);

        #endregion
    }
}
