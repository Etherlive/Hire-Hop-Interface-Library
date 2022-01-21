using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Asset : JsonObject
    {
        #region Properties

        public string barcode
        {
            get { return json.Value.GetProperty("BARCODE").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<Asset[]> Search()
        {
            return null;
        }

        #endregion Methods
    }
}