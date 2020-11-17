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
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Services {
    public class ConditionsService {
        private readonly IStorageAPI storage;

        public ConditionsService(IStorageAPI storage) {
            this.storage = storage;
        }

        public async Task<string> ToReadable(ConditionRule rule) {
            if (rule is ConditionRule.EventConditionRule eventRule) {
                var e = await storage.GetEvent(eventRule.EventID);
                return $"[{eventRule.EventID}] {e.EventValue.Value}";
            }
            else if (rule is ConditionRule.ComplexConditionRule complexRule && 1 == complexRule.Values.Count()) {
                return await ToReadable(complexRule.Values.Single());
            }
            else if (rule is ConditionRule.ConjunctionRule conjunctionRule) {
                var arms = String.Join(
                    " AND ",
                    await Task.WhenAll(
                        conjunctionRule.Values.Select(v => ToReadable(v))
                    )
                );
                return $"({arms})";
            }
            else if (rule is ConditionRule.DisjunctionRule disjunctionRule) {
                var arms = String.Join(
                    " OR ",
                    await Task.WhenAll(
                        disjunctionRule.Values.Select(v => ToReadable(v))
                    )
                );
                return $"({arms})";
            }
            else if (rule is ConditionRule.PropertyEqualityRule equalityRule) {
                var e = await storage.GetAttribute(equalityRule.PropertyID);
                return $"{e.Label} = '{equalityRule.Value}'";
            }
            else if (rule is ConditionRule.PropertyInequalityRule inequalityRule) {
                var e = await storage.GetAttribute(inequalityRule.PropertyID);
                return $"{e.Label} <> '{inequalityRule.Value}'";
            }
            else {
                throw new NotImplementedException();
            }
        }
    }
}