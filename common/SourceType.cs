﻿using System;

namespace ProgramExecutionCounter.common
{
    public class SourceType
    {
        public String Name { get; set; }

        public String Key { get; set; }

        public SourceType(String name, String key)
        {
            this.Name = name;
            this.Key = key;
        }
    }
}
