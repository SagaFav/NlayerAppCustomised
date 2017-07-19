using Domain.Datas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TestObjs:Entity
    {
        [Required]
        [MaxLength(36)]
        public string Name { get; set; }
        [Required]
        [MaxLength(36)]
        [ForeignKey("TestForeign")]
        public string ForeinKey { get; set; }
        [JsonIgnore]
        public TestForeign TestForeign { get; set; }
    }
    public class TestForeign:Entity
    {
        public string Fname { get; set; }
        [JsonIgnore]
        public ICollection<TestObjs> collection { get; set; }
    }
}
