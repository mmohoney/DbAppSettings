using System;
using System.Collections.Generic;
using System.Linq;

namespace DbAppSettings.Model.DataTransfer
{
    /// <summary>
    /// Represents a container from the data access layer to the service layer
    /// </summary>
    public class DbAppSettingDto
    {
        /// <summary>
        /// Represents data setting for a specific application
        /// </summary>
        public string ApplicationKey { get; set; }
        /// <summary>
        /// Should match the DbAppSetting.FullSettingName
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Should represent DbAppSetting.TypeString
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Returns the assembly portion of the Key
        /// </summary>
        public string Assembly
        {
            get
            {
                List<string> keySplits = Key.Split('.').ToList();
                return string.Join(".", keySplits.Take(keySplits.Count - 1));
            }
        }
        /// <summary>
        /// String representation of the DbAppSetting.Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Audit details 
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Audit details 
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Audit details 
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// Audit details 
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
