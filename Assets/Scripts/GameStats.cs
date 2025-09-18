using System.Collections.Generic;

namespace GameData
{
    [System.Serializable]
    public class GameStatsData
    {
        public int currentLevel = 0;
        public PlayerInventoryData playerInventory = new PlayerInventoryData();
        public GlobalInventoryData inventory = new GlobalInventoryData();
        public StandsData standsData = new StandsData();
    }

    [System.Serializable]
    public class PlayerInventoryData
    {
        public int capacity = 25;
        public Dictionary<FullProductId, int> content = new Dictionary<FullProductId, int>();
        public int moneyAmount = 0;
        public int level = 0;
        public int experiencePoints = 0;
    }


    [System.Serializable]
    public class GlobalInventoryData
    {
    }

    [System.Serializable]
    public class StandsData
    {
        public Dictionary<FullProductId, StandData> stands = new Dictionary<FullProductId, StandData>();
    }

    [System.Serializable]
    public class StandData
    {
        public int level = 1;
        public int basePrice;
        public int priceMultiplier = 1;
        public int inPreparation = 0;
        public int readyToSell = 0;
        public float preparationDuration;
    }
}

namespace DefaultData
{

    [System.Serializable]
    public class LevelData
    {
        public Dictionary<FullProductId, StandData> stands = new Dictionary<FullProductId, StandData>();
        public Dictionary<EFruitType, FruitData> fruits = new Dictionary<EFruitType, FruitData>();
    }

    [System.Serializable]
    public class StandData
    {
        public int basePrice;
        public int basePreparationDuration;
    }

    [System.Serializable]
    public class FruitData
    {
        public int speed;
        public int rareSpawnProbability;
        public int agressivityRate;
        public float bigVersionlootMultiplicator;
        public int loot;
    }
}