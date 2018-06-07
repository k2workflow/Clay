#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Algorithms.Tests
{
    public class GraphTests
    {
        #region Methods

        private static void AssertSetEqual<T>(HashSet<T> expected, HashSet<T> actual)
        {
            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
                actual.Remove(item);
            }

            Assert.Empty(actual);
        }

        [Fact(DisplayName = nameof(Graph_ToRepresentativeForest_Scenario1))]
        public void Graph_ToRepresentativeForest_Scenario1()
        {
            /*
                /--\  /--\           /--\
                |00+-->01<-------\---+11|
                \--/  \+-/       |   \--/
                       |         |
                /--\  /v-\  /--\ |
                |03<--+02+-->04| |
                \-+/  \+-/  \+-/ |
                  |    |     |   |
                  |   /v-\   |   |
                  \--->05<---/   |   /--\
                      \-++-------/--->12|
                        |            \--/
                      /-v\
                      |06|
                      \-+/
                /--\    |   /--\  /--\
                |07<----+--->08<--+10|
                \-+/        \+-/  \--/
                  |   /--\   |
                  \--->09<---/
                      \--/
            */

            var g = Graph.Create<int>();
            g.Add(00, 01);
            g.Add(01, 02);
            g.Add(02, 03);
            g.Add(02, 04);
            g.Add(02, 05);
            g.Add(03, 05);
            g.Add(04, 05);
            g.Add(05, 01);
            g.Add(05, 06);
            g.Add(05, 12);
            g.Add(06, 07);
            g.Add(06, 08);
            g.Add(07, 09);
            g.Add(08, 09);
            g.Add(10, 08);
            g.Add(11, 01);

            var expectedTrees = new HashSet<TreeNode<int>>()
            {
                // Tree starting at 00.
                new TreeNode<int>(00, 0),
                new TreeNode<int>(01, 0, 0),
                new TreeNode<int>(06, 0, 0, 0),
                new TreeNode<int>(07, 0, 0, 0, 0),
                new TreeNode<int>(09, 0, 0, 0, 0, 0),
                new TreeNode<int>(08, 0, 0, 0, 1),
                new TreeNode<int>(09, 0, 0, 0, 1, 0),
                new TreeNode<int>(12, 0, 0, 1), // Sibling to 06.

                // Tree starting at 10.
                new TreeNode<int>(10, 1),
                new TreeNode<int>(08, 1, 0),
                new TreeNode<int>(09, 1, 0, 0),

                // Tree starting at 11. Identical to 00 except root node.
                new TreeNode<int>(11, 2),
                new TreeNode<int>(01, 2, 0),
                new TreeNode<int>(06, 2, 0, 0),
                new TreeNode<int>(07, 2, 0, 0, 0),
                new TreeNode<int>(09, 2, 0, 0, 0, 0),
                new TreeNode<int>(08, 2, 0, 0, 1),
                new TreeNode<int>(09, 2, 0, 0, 1, 0),
                new TreeNode<int>(12, 2, 0, 1),
            };

            var expectedCycles = new HashSet<Edge<int>>()
            {
                // Nodes always contain themselves as a cycle (at minimum).
                new Edge<int>(00, 00),
                new Edge<int>(06, 06),
                new Edge<int>(07, 07),
                new Edge<int>(08, 08),
                new Edge<int>(09, 09),
                new Edge<int>(10, 10),
                new Edge<int>(11, 11),
                new Edge<int>(12, 12),

                new Edge<int>(01, 01),
                new Edge<int>(01, 02),
                new Edge<int>(01, 03),
                new Edge<int>(01, 04),
                new Edge<int>(01, 05),
            };

            var actualTrees = new HashSet<TreeNode<int>>();
            var actualCycles = new HashSet<Edge<int>>();
            g.ToRepresentativeForest(e => actualCycles.Add(e), t => actualTrees.Add(t.Clone()));

            AssertSetEqual(expectedCycles, actualCycles);
            AssertSetEqual(expectedTrees, actualTrees);
        }

        #endregion
    }
}
