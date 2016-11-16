using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Translator.Contract
{
    [DataContract]
    public class TransformTextResponse
    {
        [DataMember]
        public int ec { get; set; }
        [DataMember]
        public string em { get; set; }
        [DataMember]
        public string sentence { get; set; }
    }
}
