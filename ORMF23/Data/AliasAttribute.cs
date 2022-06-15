using System;

namespace ORMF23.Data
{
    public class AliasAttribute : Attribute
    {
        public string Name { get; set; }
        public AliasAttribute(string name)
        {
            Name = name;
        }
    }
}