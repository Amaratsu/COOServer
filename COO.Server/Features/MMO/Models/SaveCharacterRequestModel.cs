﻿namespace COO.Server.Features.MMO.Models
{
    using COO.Server.Data.Models;
    using System.Collections.Generic;

    public class SaveCharacterRequestModel
    {
        public int CharId { get; set; }
        public List<Inventory> Inventory { get; set; }
        public List<Quest> Quests { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }
        public decimal RotationYaw { get; set; }
        public string EquipChest { get; set; }
        public string EquipFeet { get; set; }
        public string EquipHands { get; set; }
        public string EquipHead { get; set; }
        public string EquipLegs { get; set; }
        public string Hotbar0 { get; set; }
        public string Hotbar1 { get; set; }
        public string Hotbar2 { get; set; }
        public string Hotbar3 { get; set; }
    }
}
