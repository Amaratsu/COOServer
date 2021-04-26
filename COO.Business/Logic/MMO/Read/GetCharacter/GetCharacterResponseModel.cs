﻿using System.Collections.Generic;
using COO.Domain.Core;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public class GetCharacterResponseModel
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public string ClanName { get; set; }
        public string AllianceName { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<Quest> Quests { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double RotationYaw { get; set; }
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
