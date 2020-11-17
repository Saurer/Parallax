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

        public string ModelsCreate(int eventBase, int parentModelID) =>
            Models() + $"/create?base={eventBase}&parent={parentModelID}";

        public string ModelsCreate(int eventBase, int parentModelID, string defaultName) =>
            Models() + $"/create?base={eventBase}&parent={parentModelID}&name={defaultName}";

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