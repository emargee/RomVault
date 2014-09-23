/******************************************************
 *     ROMVault2 is written by Gordon J.              *
 *     Contact gordon@romvault.com                    *
 *     Copyright 2014                                 *
 ******************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace ROMVault2.RvDB
{
    public class RvGame
    {

        public enum GameData
        {
            Description = 1,
            RomOf = 2,
            IsBios = 3,
            Sourcefile = 4,
            CloneOf = 5,
            SampleOf = 6,
            Board = 7,
            Year = 8,
            Manufacturer = 9,

            Trurip = 10,
            Publisher = 11,
            Developer = 12,
            Edition = 13,
            Version = 14,
            Type = 15,
            Media = 16,
            Language = 17,
            Players = 18,
            Ratings = 19,
            Peripheral = 20,
            Genre = 21,
            MediaCatalogNumber=22,
            BarCode=23
        }

        private class GameMetaData
        {
            public GameData Id { get; private set; }
            public String Value { get; private set; }

            public GameMetaData(GameData id, String value)
            {
                Id = id;
                Value = value;
            }
            public GameMetaData(BinaryReader br)
            {
                Id = (GameData)br.ReadByte();
                Value = br.ReadString();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write((byte)Id);
                bw.Write(DB.Fn(Value));
            }
        }

        private readonly List<GameMetaData> _gameMetaData = new List<GameMetaData>();


        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)_gameMetaData.Count);
            foreach (GameMetaData gameMD in _gameMetaData)
                gameMD.Write(bw);
        }

        public void Read(BinaryReader br)
        {
            byte c = br.ReadByte();
            _gameMetaData.Clear();
            _gameMetaData.Capacity = c;
            for (byte i = 0; i < c; i++)
                _gameMetaData.Add(new GameMetaData(br));
        }

        public void AddData(GameData id, string val)
        {
            if (string.IsNullOrEmpty(val))
                return;

            int pos = 0;
            while (pos < _gameMetaData.Count && _gameMetaData[pos].Id < id)
                pos++;

            _gameMetaData.Insert(pos, new GameMetaData(id, val));
        }

        public string GetData(GameData id)
        {
            foreach (GameMetaData gameMD in _gameMetaData)
            {
                if (id == gameMD.Id) return gameMD.Value;
                if (id < gameMD.Id) return "";
            }
            return "";
        }

        public void DeleteData(GameData id)
        {
            for (int i = 0; i < _gameMetaData.Count; i++)
            {
                if (id == _gameMetaData[i].Id)
                {
                    _gameMetaData.RemoveAt(i);
                    return;
                }
                if (id < _gameMetaData[i].Id) return;
            }
        }
    }

}
