using System;

namespace SandboxConsoleApp
{
    public class BlockChainGenerator
    {
        public static void BuildBlockChain()
        {
            var block1 = new Block(null, new string[] { "A gives 1 bitcoin to B" });
            var block2 = block1.NextBlock = new Block(block1, new string[] { "D gives 1 bitcoin to B" });
            var block3 = block2.NextBlock = new Block(block2, new string[] { "X gives 1 bitcoin to Y" });
            var block4 = block3.NextBlock = new Block(block3, new string[] { "Y gives 1 bitcoin to C" });
            var block5 = block4.NextBlock = new Block(block3, new string[] { "Y gives 1 bitcoin to C" });

            Console.WriteLine(block1);
            Console.WriteLine(block2);
            Console.WriteLine(block3);
            Console.WriteLine(block4);
            Console.WriteLine(block5);

            Console.WriteLine(block1);
            Console.WriteLine(block2);
            Console.WriteLine(block3);
            // A fraudulent update is made to block 3
            block3.Transactions[0] = "Y gives 1 bitcoin to A";
            
            // this breaks the chain and results in block 3, 4 and 5 becoming invalid!!
            Console.WriteLine(block4);
            Console.WriteLine(block5);

            Console.ReadKey();
        }
    }

    public class Block
    {
        public readonly Guid BlockId;
        public readonly int? PreviousBlockHash;
        public int BlockHash;
        public string[] Transactions { get; set; }
        public Block NextBlock { get; internal set; }

        public Block(Block previousBlock, string[] transactions)
        {
            BlockId = Guid.NewGuid();
            PreviousBlockHash = previousBlock?.BlockHash;
            // Create new Block using previous Hash and Transactions for block
            BlockHash = (new object[] { transactions, PreviousBlockHash }).GetHashCode();
        }

        // The magic of the block chain
        public bool IsBlockValid()
        {
            if (NextBlock == null) return false;
            return NextBlock.BlockHash.Equals((new object[] { NextBlock.Transactions, BlockHash }).GetHashCode());
        }

        public override string ToString()
        {
            return $"Block Id is {BlockId} - BlockHash {BlockHash} : IsVald {IsBlockValid()}";
        }
    }
}
