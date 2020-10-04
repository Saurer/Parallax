using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AuroraCore.Storage;

namespace Parallax.Models {
    public enum ConditionType {
        Conjunction,
        Disjunction,
        Equality,
        Inequality
    }

    public class ConditionData {
        public ConditionType Rule { get; set; }

        [Required]
        public int Property { get; set; }
        public string Value { get; set; }

        public List<ConditionData> Items { get; private set; }

        public bool IsCollection {
            get {
                switch (Rule) {
                    case ConditionType.Conjunction:
                    case ConditionType.Disjunction:
                        return true;

                    case ConditionType.Equality:
                    case ConditionType.Inequality:
                        return false;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public ConditionData() {
            Items = new List<ConditionData>();
        }

        public ConditionRule Compile() {
            return Traverse(this);
        }

        private ConditionRule Traverse(ConditionData data) {
            return data.Rule switch
            {
                ConditionType.Conjunction => new ConditionRule.ConjunctionRule(
                    data.Items.Select(Traverse)
                ),
                ConditionType.Disjunction => new ConditionRule.DisjunctionRule(
                    data.Items.Select(Traverse)
                ),
                ConditionType.Equality => new ConditionRule.PropertyEqualityRule(data.Property, data.Value),
                ConditionType.Inequality => new ConditionRule.PropertyInequalityRule(data.Property, data.Value),
                _ => throw new NotImplementedException()
            };
        }
    }
}