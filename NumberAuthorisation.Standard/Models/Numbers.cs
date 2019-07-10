using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NumberAuthorisation.Standard;
using NumberAuthorisation.Standard.Utilities;


namespace NumberAuthorisation.Standard.Models
{
    public class Numbers : BaseModel 
    {
        // These fields hold the values for the public properties.
        private List<string> numbers;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("numbers")]
        public List<string> NumbersProp 
        { 
            get 
            {
                return this.numbers; 
            } 
            set 
            {
                this.numbers = value;
                onPropertyChanged("NumbersProp");
            }
        }
    }
} 