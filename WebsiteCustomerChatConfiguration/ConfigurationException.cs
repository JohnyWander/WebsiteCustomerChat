using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCustomerChatConfiguration
{
    /// <summary>
    /// Occurs when configuration file has wrong data, when configuration couldn't be parsed,<\br>
    /// or when parsing throws other exceptions, those will be in inner exception
    /// </summary>
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
        {

        }

        public ConfigurationException(string message) : base(message) { }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }


    }
}
