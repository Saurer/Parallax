// Parallax
// Copyright (C) 2020  Frank Horrigan <https://github.com/saurer>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
            return data.Rule switch {
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