using System.Collections.Generic;

namespace WebsiteCustomerChatConfiguration
{
    public struct ConfigData
    {
        public string Name;
        public string Value;
        public string Description;

        public ConfigData(string name, string value,string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
    }


    public static class Config
    {
        private const char _desc = '|';
        public static IDictionary<string, ConfigData> ConfigDictionary = new Dictionary<string, ConfigData>
        {
            {"DatabaseEngine",
                new ConfigData
                (
                    name:"DatabaseEngine",
                    value:"",
                    description:"Database engine used by the app, value is assigned during installation, however you may change that later on."

                )
            }

        };







    }
}