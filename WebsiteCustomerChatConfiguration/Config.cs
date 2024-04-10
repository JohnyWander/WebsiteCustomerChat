﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
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

        public static List<ConfigData> ParsedConfiguration = new List<ConfigData>();

        public static List<ConfigData> Configuration {
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

            string[] fileLines = File.ReadAllLines(configFilename);

            List<string> configLoaded =  fileLines.Where(s=>!s.StartsWith(_desc)).Where(s=>!string.IsNullOrEmpty(s)).ToList(); //filter out commented values
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
                InvalidConfigNames.ForEach(x => { Debug.WriteLine(x); ExceptionMessage.AppendLine($"Invalid configuration - {x} at line {fileLines.ToList().FindIndex(s => s.StartsWith(x))} "); }); ; ;
                ExceptionMessage.AppendLine("Please check readme for valid configuration options");
                Debug.WriteLine(ExceptionMessage.ToString());
                throw new ConfigurationException(ExceptionMessage.ToString());
            }
            else
            {

                configLoaded.ForEach(x =>
                {
                    string[] split = x.Split("=");
                    string name = split[0];
                    string value = split[1];

                    Debug.WriteLine(name);

                    ConfigData ConfigToLoad = ConfigDictionary.Where(x => x.Name == name).Single();



                    List<string> FailedConstraints = new List<string>();

                    if (ConfigToLoad.ValueConstraints is not null) { 

                        ConfigToLoad.ValueConstraints.ToList().ForEach(x =>
                        {
                            if (!x(ConfigToLoad.Value))
                            {
                                FailedConstraints.Add($"Configuration constraint- {x.Method.Name} for conf name {ConfigToLoad.Name} and it's value {value}");
                            }
                        });

                        if (FailedConstraints.Count != 0)
                        {
                            string exceptionMessage = "";
                            FailedConstraints.ForEach(x => { exceptionMessage += x + "\n"; });
                            throw new ConfigurationException(exceptionMessage);
                        }
                        else
                        {
                            ParsedConfiguration.Add(new ConfigData(ConfigToLoad.Name,value,ConfigToLoad.Description));
                        }

                    }
                    else
                    {
                        ParsedConfiguration.Add(new ConfigData(ConfigToLoad.Name, value, ConfigToLoad.Description));
                    }

                });
                



               
            }

           

            
        }


        public static async Task BuildConfigFile()
        {
            StreamWriter writer = new StreamWriter(configFilename);
            await writer.WriteLineAsync(_desc+"This is config file for Website CustomerChat");

            foreach(ConfigData data in ConfigDictionary)
            {
                await writer.WriteLineAsync(_desc + data.Description);
                await writer.WriteLineAsync($"{data.Name}={data.Value}\n");
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