using Skyblivion.ESReader.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader.Struct
{
    /*
     * DFS-based iterator for Trie
     * Class TrieIterator
     * @package Skyblivion\ESReader\Struct
     */
    public class TrieIterator<T> where T : class
    {
        public int Key { get; private set; }
        private readonly Trie<T>? root;
        private Trie<T>? current;
        private Stack<Trie<T>>? stack;
        /*
        * TrieIterator constructor.
        */
        public TrieIterator(Trie<T>? root)
        {
            this.root = root;
            if (null != this.root)
            {
                this.Rewind();
            }
        }

        public object? _current()
        {
            if (null != this.current)
            {
                return this.current._value();
            }
            return null;
        }

        public void Next()
        {
            if (this.current == null) { throw new NullableException(nameof(this.current)); }
            /*
             * Expand the current node to children
             */
            foreach (var subnode in this.current.Subnodes())
            {
                this.PushNodeForIteration(subnode.Value);
            }

            this.PopNodeForIteration();
            this.Key++;
        }

        public bool Valid()
        {
            return this.current != null;
        }

        public void Rewind()
        {
            this.Key = 0;
            this.stack = new Stack<Trie<T>>();
            if (null != this.root)
            {
                this.PushNodeForIteration(this.root);
                this.PopNodeForIteration();
            }
        }

        private void PushNodeForIteration(Trie<T> trie)
        {
            if (this.stack == null) { throw new NullableException(nameof(this.stack)); }
            /*
             * There can be intermediary nodes that weren"t directly inserted
             * They won"t have a value, so let"s skip them
             */
            Stack<Trie<T>[]> nodesToTravel = new Stack<Trie<T>[]>();
            nodesToTravel.Push(new Trie<T>[] { trie });
            while (nodesToTravel.Any())
            {
                Trie<T>[] currentNodes = nodesToTravel.Pop();
                foreach (var currentNode in currentNodes)
                {
                    if (null != currentNode._value())
                    {
                        this.stack.Push(currentNode);
                    }
                    else
                    {
                        nodesToTravel.Push(currentNode.Subnodes().Select(kvp=>kvp.Value).ToArray());
                    }
                }
            }
        }

        private void PopNodeForIteration()
        {
            if (this.stack == null) { throw new NullableException(nameof(this.stack)); }
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