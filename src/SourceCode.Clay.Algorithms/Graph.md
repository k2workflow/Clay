# Graph

## Representative Forest

This algorithm allows a set of representative trees to be constructed from a
directed cyclic graph, which can be used to answer queries such as:

 * Descendants of this node.
 * Descendants and this node.
 * Ancestors of this node.
 * Ancestors and this node.

It works by first tagging all nodes that form part of a cycle. The tag is the
first node encountered in the cycle. Cycles are then removed from the graph
and stored seperately, while being replaced with a single node (sharing the
same value as the tag). This forms a directed acyclic graph. Finally, the
directed acyclic graph is decomposed into a forest. Note that nodes that are
not in a cycle are still marked as part of one (containing only themselves).

This approach works because, for ancestor/descendant queries only, all nodes in
a cycle are ancestors and descendants of eachother.

This structure is suitable for storage in SQL. The cycles and forest should be
stored in seperate tables. The query always begins at the cycle table, joining
to the forest table (finding descendants using e.g. [ORDPATH][1]) and back again
to the cycle table.

[1]: http://www.dbis.informatik.hu-berlin.de/fileadmin/lectures/WS2011_12/NeueKonzepte_Vorlesung/ordpath.pdf "ORDPATHs: Insert-Friendly XML Node Labels"