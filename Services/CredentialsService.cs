using AuroraCore.Storage;

namespace Parallax.Services {
    public class CredentialsService {

        public IIndividual CurrentActor { get; private set; }

        public void SetCurrentActor(IIndividual actor) {
            CurrentActor = actor;
        }
    }
}