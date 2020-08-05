namespace Parallax.Services {
    public class RoutingService {
        public string Attributes() =>
            "/attributes";

        public string AttributesCreate() =>
            Attributes() + "/create";

        public string AttributesView(int id) =>
            Attributes() + $"/{id}";

        public string Actors() =>
            "/actors";

        public string ActorsCreate() =>
            Actors() + "/create";

        public string Entities() =>
            "/entities";

        public string EntitiesCreate() =>
            Entities() + "/create";

        public string EntitiesView(int id) =>
            Entities() + $"/{id}";

        public string EntitiesCreateIndividual(int entityID) =>
            Entities() + $"/{entityID}/create-individual";

        public string EntitiesCreateIndividual(int entityID, int modelID) =>
            Entities() + $"/{entityID}/create-individual?model={modelID}";

        public string Models() =>
            "/models";

        public string ModelsView(int id) =>
            Models() + $"/{id}";

        public string Roles() =>
            "/roles";

        public string RolesCreate() =>
            Roles() + "/create";

        public string Relations() =>
            "/relations";

        public string RelationsCreate() =>
            Relations() + "/create";

        public string Individuals() =>
            "/individuals";

        public string IndividualsView(int id) =>
            Individuals() + $"/{id}";
    }
}