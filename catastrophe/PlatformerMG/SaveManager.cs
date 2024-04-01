using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Catastrophe
{
    public class SaveDataInfo
    {
        private static SaveDataInfo mInstance = null;
        public static SaveDataInfo Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new SaveDataInfo();
                return mInstance;
            }

            set { mInstance = value; }
        }

        public int Level { get; set; }
        public List<int> highScores { get; set; }
    }
    class SaveManager
    {
        public void LoadSavedData()
        {
            var filecontent = File.ReadAllText("Savedata.json");
            SaveDataInfo.Instance = JsonSerializer.Deserialize<SaveDataInfo>(filecontent);
        }
        public void SaveData()
        {
            string Serializedtext = JsonSerializer.Serialize<SaveDataInfo>(SaveDataInfo.Instance);
            Trace.WriteLine(Serializedtext);
            File.WriteAllText("Savedata.json", Serializedtext);
        }
    }
}
