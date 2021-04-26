﻿namespace COO.Domain.Core
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Experience { get; set; }
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
        public bool IsOnline { get; set; }

        public int UserId { get; set; }

        public int GameServerId { get; set; }

        public UserCharacter UserCharacter { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

        public int? ClanId { get; set; }
        public Clan Clan { get; set; }

        public int? AllianceId { get; set; }
        public Alliance Alliance { get; set; }
    }
}
