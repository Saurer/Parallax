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