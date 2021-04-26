using System.Collections.Generic;

namespace Clonebit
{
    public class Settings
    {
        public static List<string> Serials;

        public List<string> Addresses { get; set; }
        public string PrivateKeyPath { get; set; }
        public string NFSPath { get; set; }
        public string LogsPath { get; set; }
    }
}