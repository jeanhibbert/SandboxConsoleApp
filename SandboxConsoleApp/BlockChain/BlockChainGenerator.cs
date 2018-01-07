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
            blockChain.Add(new List<string> { "A gives 1 bitcoin to B" });
            blockChain.Add(new List<string> { "D gives 1 bitcoin to B" });
            blockChain.Add(new List<string> { "X gives 1 bitcoin to Y" });
            blockChain.Add(new List<string> { "Y gives 1 bitcoin to C" });
            blockChain.Add(new List<string> { "X gives 1 bitcoin to A" });

            Console.WriteLine(blockChain.IsValid);

            // A fraudulent update is made to block 3
            blockChain[3].Transactions.Add ("Y gives 1 bitcoin to A");

            Console.WriteLine(blockChain.IsValid);

            Console.ReadKey();
        }
    }

    public interface IBlockChain
    {
        bool IsValid { get; }
        bool Add(List<string> transactions);
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

        public bool Add(List<string> transactions)
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
        
    }

    public class Block
    {
        public readonly Guid BlockId;
        public int BlockHash;

        public List<string> Transactions { get; }
        public Block NextBlock { get; internal set; }
        public Block PreviousBlock { get; internal set; }

        private int _previousHash;

        public Block(Block previousBlock, List<string> transactions)
        {
            BlockId = Guid.NewGuid();
            PreviousBlock = previousBlock;
            _previousHash = previousBlock == null ? default(int) : previousBlock.BlockHash;

            Transactions = transactions;

            // Create new Block using previous Hash and Transactions for block
            BlockHash = (new object[] { transactions, _previousHash }).GetHashCode();
        }

        // The magic of the block chain
        public bool IsBlockValid()
        {
            if (PreviousBlock == null) return true; // Its the first block in the chain so it's automatically valid
            return !BlockHash.Equals((new object[] { Transactions, PreviousBlock.BlockHash }).GetHashCode());
        }

        public override string ToString()
        {
            return $"Block Id is {BlockId} - BlockHash {BlockHash} : IsVald {IsBlockValid()}";
        }
    }
}
