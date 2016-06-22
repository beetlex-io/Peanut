using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class AggregationAttribute:Attribute
    {
        public AggregationAttribute(AggregationType at)
        {
            Type = at;
            DISTINCT = false;
        }
        public  AggregationAttribute(AggregationType at,bool dist)
        {
            Type = at;
            DISTINCT = dist;
        }
        public AggregationType Type
        {
            get;
            set;
        }
        public bool DISTINCT
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Count : AggregationAttribute
    {
        public Count() : base(AggregationType.COUNT) { }
        public Count(bool dist) : base(AggregationType.COUNT, dist) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Sum : AggregationAttribute
    {
        public Sum() : base(AggregationType.SUM) { }
        public Sum(bool dist) : base(AggregationType.SUM, dist) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Max : AggregationAttribute
    {
         public Max() : base(AggregationType.MAX) { }
         public Max(bool dist) : base(AggregationType.MAX, dist) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Min : AggregationAttribute
    {
        public Min() : base(AggregationType.MIN) { }
        public Min(bool dist) : base(AggregationType.MIN, dist) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Avg : AggregationAttribute
    {
        public Avg() : base(AggregationType.AVG) { }
        public Avg(bool dist) : base(AggregationType.AVG, dist) { }
    }

}

