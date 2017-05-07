using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SakabaWeb
{
    public class Boss
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int LifePoint { get; set; } = 1;
        public int EvadeRate { get; set; }
        public string VoiceAppear { get; set; }
        public string VoiceDamage { get; set; }
        public string VoiceCounter { get; set; }
        public string VoiceDead { get; set; }
        public string DropItem { get; set; }
        public string Weakness { get; set; }

        public string User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}