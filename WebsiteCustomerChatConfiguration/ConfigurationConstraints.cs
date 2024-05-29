namespace WebsiteCustomerChatConfiguration
{
    public static class ConfigurationConstraints
    {
        public static bool IsDecimal(string value = "")
        {
            return int.TryParse(value, out _);
        }

        public static bool IsBoolean(string value = "")
        {
            return bool.TryParse(value, out _);
        }


    }
}
