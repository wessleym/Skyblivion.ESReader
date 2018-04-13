using Skyblivion.ESReader.PHP;
using Skyblivion.ESReader.TES4;
using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.Struct
{
    /*
     * Class Trie
     * Based off implementation of https://github.com/fran6co/phptrie/
     */
    public class Trie
    {
        private Dictionary<string, Trie> trie = new Dictionary<string, Trie>();
        private TES4LoadedRecord value = null;
        /*
        * Trie constructor
         *
         *  This is for internal use
        */
        public Trie(TES4LoadedRecord value = null)
        {
            this.value = value;
        }

        /*
        * Add value to the trie
         *
         *  string The key
         *  mixed The value
         *  Overwrite existing value
        */
        public void add(string str, TES4LoadedRecord value, bool overWrite = true)
        {
            if (str == "")
            {
                if (this.value == null || overWrite) { this.value = value; }
                return;
            }
            foreach (var kvp in this.trie)
            {
                var prefix = kvp.Key;
                var trie = kvp.Value;
                int prefixLength = prefix.Length;
                string head = PHPFunction.Substr(str, prefixLength);
                int headLength = head.Length;
                bool equals = true;
                string equalPrefix = "";
                for (int i = 0; i < prefixLength; ++i)
                { //Split
                    if (i >= headLength)
                    {
                        Trie equalTrie = new Trie(value);
                        this.trie.Add(equalPrefix, equalTrie);
                        equalTrie.trie.Add(prefix.Substring(i), trie);
                        this.trie.Remove(prefix);
                        return;
                    }
                    else if (prefix[i] != head[i])
                    {
                        if (i > 0)
                        {
                            Trie equalTrie = new Trie();
                            this.trie.Add(equalPrefix, equalTrie);
                            equalTrie.trie.Add(prefix.Substring(i), trie);
                            equalTrie.trie.Add(str.Substring(i), new Trie(value));
                            this.trie.Remove(prefix);
                            return;
                        }

                        equals = false;
                        break;
                    }

                    equalPrefix += head[i];
                }

                if (equals)
                {
                    trie.add(str.Substring(prefixLength), value, overWrite);
                    return;
                }
            }
            this.trie.Add(str, new Trie(value));
        }

        /*
        * Search the Trie with a string
        */
        public TES4LoadedRecord search(string str)
        {
            if (str == "")//WTM:  Change:  In PHP, this was empty(str), which returns true when str = "0"
            {
                return this.value;
            }
            foreach (var kvp in this.trie)
            {
                var prefix = kvp.Key;
                if (str.StartsWith(prefix))
                {
                    var trie = kvp.Value;
                    return trie.search(str.Substring(prefix.Length));
                }
            }
            return null;
        }

        public TrieIterator searchPrefix(string str)
        {
            if (str == "") { return new TrieIterator(this); }
            int stringLength = str.Length;
            foreach (var kvp in this.trie)
            {
                var prefix = kvp.Key;
                var trie = kvp.Value;
                int prefixLength = prefix.Length;
                string headPrefix, stringPrefix;
                if (prefixLength > stringLength)
                {
                    headPrefix = prefix.Substring(0, stringLength);
                    stringPrefix = str;
                }
                else if (prefixLength < stringLength)
                {
                    headPrefix = prefix;
                    stringPrefix = str.Substring(0, prefixLength);
                }
                else
                {
                    headPrefix = prefix;
                    stringPrefix = str;
                }

                if (headPrefix == stringPrefix)
                {
                    return trie.searchPrefix(str.Substring(prefixLength));
                }
            }
            return new TrieIterator(null);
        }

        public TES4LoadedRecord _value()
        {
            return this.value;
        }

        public Dictionary<string, Trie> subnodes()
        {
            return this.trie;
        }
    }
}