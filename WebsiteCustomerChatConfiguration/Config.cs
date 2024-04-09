using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static WebsiteCustomerChatConfiguration.ConfigurationConstraints;

namespace WebsiteCustomerChatConfiguration
{
    public struct ConfigData
    {
        public string Name;
        public string Value;
        public string Description;

        public  Predicate<string>[] ?ValueConstraints = null;
        
        public ConfigData(string name, string value,string description,params Predicate<string>[] constrains)
        {
            Name = name;
            Value = value;
            Description = description;
            
        }
    }


    public static class Config
    {
        public const char _desc = '|';
        public const string configFilename = "WebsiteCustomerChat.conf";

        private static ConfigData[] ?ParsedConfiguration;

        public static ConfigData[] Configuration {
            get
            {
                if (ParsedConfiguration is not null)
                    return ParsedConfiguration;
                else
                    throw new NullReferenceException("Configuration is null, or was not parsed corretly");

            }
        }

        public static async Task ParseConfig()
        {
            string[] validconfigNames = ConfigDictionary.Select(x=>x.Name).ToArray();

            
            List<string> configLoaded =  File.ReadAllLines(configFilename).Where(s=>!s.StartsWith(_desc)).ToList(); //filter out commented values
            List<string> configNames = new List<string>();
            List<string> configValues = new List<string>();
            configLoaded.ForEach(x =>
            {
                string[] split = x.Split('=');
                configNames.Add(split[0]);
            });


            if (!configNames.All(validconfigNames.Contains))
            {
                Debug.WriteLine("NOT OK");
                List<string> InvalidConfigNames = configNames.Except(validconfigNames.ToList()).ToList();
                StringBuilder ExceptionMessage = new StringBuilder();
                InvalidConfigNames.ForEach(x=> ExceptionMessage.AppendLine($"Invalid configuration - {x} at line{InvalidConfigNames.IndexOf(x)}"));
                ExceptionMessage.AppendLine("Please check readme for valid configuration options");
                throw new ConfigurationException(ExceptionMessage.ToString());
            }
            else
            { 
             
               
            }

           

            
        }


        public static async Task BuildConfigFile()
        {
            StreamWriter writer = new StreamWriter(configFilename);
            await writer.WriteLineAsync(_desc+"This is config file for Website CustomerChat");

            foreach(ConfigData data in ConfigDictionary)
            {
                await writer.WriteLineAsync(_desc + data.Description);
                await writer.WriteLineAsync($"{data.Name}={data.Value}");
            }

            await writer.DisposeAsync();
        }


        public static List<ConfigData> ConfigDictionary = new List<ConfigData>
            {
            new ConfigData
                (
                    name:"DatabaseEngine",
                    value:"",
                    description:"Database engine used by the app, value is assigned during installation, however you may change that later on."
                ),
            new ConfigData
                (
                    name:"DatabaseUser",
                    value:"",
                    description:"User used to login to the database, not used for sglite engine."
                ),
            new ConfigData
                (
                    name:"DatabasePassword",
                    value:"",
                    description:"User database password"
                ),
            new ConfigData
                (
                    name:"DatabaseHost",
                    value:"localhost",
                    description:"Host address of the database"
                ),
           new ConfigData
                (
                    name:"DatabasePort",
                    value:"3306",
                    description:"Port on which database listens",
                    constrains: (_)=>IsDecimal()

                )

            };

}
        







    
}