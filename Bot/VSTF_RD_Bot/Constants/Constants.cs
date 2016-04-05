using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VSTF_RD_Bot
{
    public static class Constants
    {
        #region LUIS Constants

        internal static string Type_Entity = "Type";

        internal static string Query_Entity = "Query";

        internal static string Title_Entity = "Title";

        internal const string LUIS_AppId = "Your APP ID";
        internal const string LUIS_AppSecret = "Your App Secret";

        #endregion

        #region

        internal static string LogicAppQueryUrl = ConfigurationManager.AppSettings["logicAppQueryUrl"];
        internal static string LogicAppCreateUrl = ConfigurationManager.AppSettings["logicAppCreateUrl"];
        internal static string LogicAppCommandUrl = ConfigurationManager.AppSettings["logicAppCommandUrl"];

        internal static string ADClientId = ConfigurationManager.AppSettings["ADClientId"];

        internal static string ADClientSecrent = ConfigurationManager.AppSettings["ADClientSecret"];

        internal static string apiBasePath = ConfigurationManager.AppSettings["apiBasePath"].ToLower();

        internal static string botId = ConfigurationManager.AppSettings["AppId"];

        internal static string botSecret = ConfigurationManager.AppSettings["AppSecret"];
        internal static string regex_create = "\\s(.*):\\s?(.*)";
        internal static string regex_command = "^\\/(\\w*)\\s*";

        #endregion
    }

}