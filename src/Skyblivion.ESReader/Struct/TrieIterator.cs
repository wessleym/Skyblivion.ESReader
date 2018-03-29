using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader.Struct
{
    /*
     * DFS-based iterator for Trie
     * Class TrieIterator
     * @package Skyblivion\ESReader\Struct
     */
    public class TrieIterator
    {
        private int k;
        private Trie root;
        private Trie current;
        private Stack<Trie> stack;
        /*
        * TrieIterator constructor.
        */
        public TrieIterator(Trie root)
        {
            this.root = root;
            if (null != this.root)
            {
                this.rewind();
            }
        }

        public object _current()
        {
            if (null != this.current)
            {
                return this.current._value();
            }
            return null;
        }

        public void next()
        {
            /*
             * Expand the current node to children
             */
            foreach (var subnode in this.current.subnodes())
            {
                this.pushNodeForIteration(subnode.Value);
            }

            this.popNodeForIteration();
            this.k++;
        }

        public int key()
        {
            return this.k;
        }

        public bool valid()
        {
            return this.current != null;
        }

        public void rewind()
        {
            this.k = 0;
            this.stack = new Stack<Trie>();
            if (null != this.root)
            {
                this.pushNodeForIteration(this.root);
                this.popNodeForIteration();
            }
        }

        private void pushNodeForIteration(Trie trie)
        {
            /*
             * There can be intermediary nodes that weren"t directly inserted
             * They won"t have a value, so let"s skip them
             */
            Stack<Trie[]> nodesToTravel = new Stack<Trie[]>();
            nodesToTravel.Push(new Trie[] { trie });
            while (nodesToTravel.Any())
            {
                Trie[] currentNodes = nodesToTravel.Pop();
                foreach (var currentNode in currentNodes)
                {
                    if (null != currentNode._value())
                    {
                        this.stack.Push(currentNode);
                    }
                    else
                    {
                        nodesToTravel.Push(currentNode.subnodes().Select(kvp=>kvp.Value).ToArray());
                    }
                }
            }
        }

        private void popNodeForIteration()
        {
            /*
             * Pop the next node
             */
            if (this.stack.Any())
            {
                this.current = this.stack.Pop();
            }
            else
            {
                this.current = null;
            }
        }
    }
}