using Template.Domain.ValueObjects.Base;

namespace Template.Domain.ValueObjects
{
    public class PlatformVo : ValueObject<PlatformVo>
    {
        #region Constructors | Destructors
        public PlatformVo(string device, string version, string model, string cordova)
        {
            Device = device;
            Version = version;
            Model = model;
            Cordova = cordova;
        }
        #endregion

        #region Properties
        public string Device { get; private set; }

        public string Version { get; private set; }

        public string Model { get; private set; }

        public string Cordova { get; private set; }
        #endregion
    }
}
