using Microsoft.AspNetCore.Identity;
using VerifyStringUtility;
using EncryptionUtility;
namespace Services
{
    public class Service
    {
        private string _name { get; set; }
        private string _guid { get; set; }
        private string _encrypted_password { get; set; }
        private string _creation_date { get; set; }


        // public Service(string name)
        // {
        //     if (!VerifyStringUtil.isValidUsername(name))
        //         throw new ArgumentException("Error: ", nameof(name));

        //     string guid = VerifyStringUtil.CreateGuid(); ;

        //     if (!VerifyStringUtil.isValidGuid(guid))
        //         throw new ArgumentException("Guid error", nameof(guid));

        //     this._name = name;
        //     this._guid = guid;
        //     this._creation_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //     this._guid = VerifyStringUtil.CreateGuid();
        // }
        public Service(string name, string guid, string password)
        {
            if (!VerifyStringUtil.isValidUsername(name))
                throw new ArgumentException("Error: ", nameof(name));

            if (!VerifyStringUtil.isValidGuid(guid))
                throw new ArgumentException("Error: ", nameof(guid));

            this._name = name;
            this._guid = guid;
            this._creation_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this._guid = VerifyStringUtil.CreateGuid();
            this._encrypted_password = EncryptionUtil.EncryptString(password);
        }

        public string Name { get => _name; }
        public string Guid { get => _guid; }
        public string EncryptedPassword { get => _encrypted_password; }
        public string CreationDate { get => _creation_date; }


    }

}
