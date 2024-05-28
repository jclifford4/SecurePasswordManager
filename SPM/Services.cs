using Microsoft.AspNetCore.Identity;
using VerifyStringUtility;
namespace Services
{
    public class Service
    {
        private string _name { get; set; }
        private string _guid { get; set; }


        public Service(string name)
        {
            if (!VerifyStringUtil.isValidUsername(name))
                throw new ArgumentException("Error: ", nameof(name));

            string guid = VerifyStringUtil.CreateGuid(); ;

            if (!VerifyStringUtil.isValidGuid(guid))
                throw new ArgumentException("Guid error", nameof(guid));

            this._name = name;
            this._guid = guid;
        }
        public Service(string name, string guid, string password)
        {
            if (!VerifyStringUtil.isValidUsername(name))
                throw new ArgumentException("Error: ", nameof(name));

            if (!VerifyStringUtil.isValidGuid(guid))
                throw new ArgumentException("Error: ", nameof(guid));

            this._name = name;
            this._guid = guid;
        }

        public string Name { get => _name; }
        public string Guid { get => _guid; }


    }

}
