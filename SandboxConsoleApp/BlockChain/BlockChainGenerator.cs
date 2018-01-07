using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SandboxConsoleApp
{
    public class BlockChainGenerator
    {
        public static void BuildBlockChain()
        {
            var blockChain = new BlockChain();

            blockChain.Add(new string[] { "A gives 1 bitcoin to B" });
            blockChain.Add(new string[] { "D gives 1 bitcoin to B" });
            blockChain.Add(new string[] { "X gives 1 bitcoin to Y" });
            blockChain.Add(new string[] { "Y gives 1 bitcoin to C" });
            blockChain.Add(new string[] { "X gives 1 bitcoin to A" });

            Console.WriteLine(blockChain.IsValid);

            // A fraudulent update is made to block 3
            blockChain[3].Transactions = new string[] { "Y gives 1 bitcoin to A" };
            //Console.WriteLine(blockChain.IsValid);

            blockChain.DisplayBlocks();

            Console.ReadKey();
        }
    }

    public interface IBlockChain
    {
        bool IsValid { get; }
        bool Add(string[] transactions);
    }

    public class BlockChain : IBlockChain
    {
        private ConcurrentDictionary<int, Block> _chain = new ConcurrentDictionary<int, Block>();

        public Block this[int index]
        {
            get
            {
                return _chain[index];
            }
        }

        public bool IsValid
        {
            get
            {
                return _chain.All(x => x.Value.IsBlockValid());
            }
        }

        public bool Add(string[] transactions)
        {
            Block block;
            if (_chain.IsEmpty)
            {
                // This is the first block in the blockchain
                block = new Block(null, transactions);
            }
            else
            { 
                var previousBlock = _chain[_chain.Count - 1];
                block = new Block(previousBlock, transactions);
                previousBlock.NextBlock = block;
            }
            return _chain.TryAdd(_chain.Count, block);
        }

        public void DisplayBlocks()
        {
            foreach(var kv in _chain)
            {
                Console.WriteLine(kv.Value);
            }
        }
        
    }

    public class Block
    {
        public string[] Transactions;
        public Block NextBlock { get; internal set; }
        public object[] Contents { get; }

        private readonly int _previousBlockHash;
        private readonly int _blockHash;

        public Block(Block previousBlock, string[] transactions)
        {
            // All truth is defined during block creation
            _previousBlockHash = previousBlock == null ? default(int) : previousBlock.BlockHash;
            Transactions = transactions;
            Contents = new object[] { Transactions, _previousBlockHash };
            _blockHash = Contents.GetHashCode();
            Console.WriteLine($"{Transactions} + {_previousBlockHash} = {_blockHash}");
        }

        // The magic of the block chain
        public bool IsBlockValid()
        {
            // Its the first block in the chain so it's automatically valid
            if (_previousBlockHash == default(int)) return true;

            // This is the last block in the chain so it must be valid i.e. we have to trust it's contents
            if (NextBlock == null) return true;

            // refresh the contents of the object array
            Contents[0] = Transactions;
            Contents[1] = _previousBlockHash;

            var nextBlockPreviousHash = Contents.GetHashCode() == NextBlock._previousBlockHash;

            var nextBlockHash = NextBlock.BlockHash == (NextBlock.Contents).GetHashCode();

            if (nextBlockPreviousHash && nextBlockHash) return true;

            return false;
        }
        public int BlockHash { get { return _blockHash; } }

        public override string ToString()
        {
            return $"BlockHash {BlockHash} : IsVald {IsBlockValid()}";
        }
    }
}
