using System.Collections.Generic;

namespace Parallax.Models {
    public class PropertyProviderData {
        private Dictionary<int, AttachedAttrData> attributes;
        private Dictionary<int, AttachedRelationData> relations;

        public int? ProviderID { get; private set; }
        public IReadOnlyDictionary<int, AttachedAttrData> Attributes => attributes;
        public IReadOnlyDictionary<int, AttachedRelationData> Relations => relations;

        public PropertyProviderData() {
            attributes = new Dictionary<int, AttachedAttrData>();
            relations = new Dictionary<int, AttachedRelationData>();
        }

        public PropertyProviderData(
            IReadOnlyDictionary<int, AttachedAttrData> attributes,
            IReadOnlyDictionary<int, AttachedRelationData> relations,
            int providerID
        ) {
            this.attributes = new Dictionary<int, AttachedAttrData>(attributes);
            this.relations = new Dictionary<int, AttachedRelationData>(relations);
            this.ProviderID = providerID;
        }

        public void AddAttribute(AttachedAttrData attr) {
            attributes.Add(attr.Attribute.ID, attr);
        }

        public void AddRelation(AttachedRelationData relation) {
            relations.Add(relation.Relation.PropertyID, relation);
        }

        public void RemoveAttribute(AttachedAttrData attr) {
            attributes.Remove(attr.Attribute.ID);
        }

        public void RemoveRelation(AttachedRelationData relation) {
            relations.Remove(relation.Relation.PropertyID);
        }
    }
}