using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class TransportPolicy : IPolicy
    {

        [JsonProperty("deny_rooted_jailbroken", NullValueHandling=NullValueHandling.Ignore)]
        public bool? DenyRootedJailbroken { get; set; }

        [JsonProperty("deny_emulator_simulator", NullValueHandling=NullValueHandling.Ignore)]
        public bool? DenyEmulatorSimulator { get; set; }

        [JsonProperty("fences", NullValueHandling=NullValueHandling.Ignore)]
        public List<Fence> Fences { get; set; }

        [JsonProperty("inside", NullValueHandling=NullValueHandling.Ignore)]
        public IPolicy Inside { get; set; }

        [JsonProperty("outside", NullValueHandling=NullValueHandling.Ignore)]
        public IPolicy Outside { get; set; }

        [JsonProperty("type", NullValueHandling=NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("amount", NullValueHandling=NullValueHandling.Ignore)]
        public int? Amount { get; set; }
        
        [JsonProperty("factors", NullValueHandling=NullValueHandling.Ignore)]
        public List<string> Factors { get; set; }
        
        // TODO: Where does this transformation happen?
        // [JsonProperty("is_inherence")]
        // public bool IsInherence { get; set; }
        // [JsonProperty("is_knowledge")]
        // public bool IsKnowledge { get; set; }
        // [JsonProperty("is_possession")]
        // public bool IsPossession { get; set; }


        public TransportPolicy(
            bool? denyRootedJailbroken = null,
            bool? denyEmulatorSimulator = null,
            List<Fence> fences = null,
            IPolicy inside = null,
            IPolicy outside = null,
            string type = null,
            int? amount = null,
            List<string> factors = null
            )
        {
            /* Add Json Parsing */
            /* Add Everything */
            /* Dont send attributes that are Null */

            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences;
            Inside = inside;
            Outside = outside;
            Type = type;
            Amount = amount;
            Factors = factors;

            // Is this the right place to put this?
            foreach(string factor in factors)
            {
                switch (factor)
                {
                    case "KNOWLEDGE":
                        IsKnowledge = true;
                        break;
                    case "POSSESSION":
                        IsPossession = true;
                        break;
                    case "INHERENCE":
                        IsInherence = true;
                        break;
                }
            }

        }

        public bool IsInherence { get; set; }
        public bool IsKnowledge { get; set; }
        public bool IsPossession { get; set; }

    }
}
