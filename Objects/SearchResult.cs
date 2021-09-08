using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class SearchResult
    {
        #region Fields

        public JObject data;

        #endregion Fields

        #region Constructors

        public SearchResult(JToken _data)
        {
            data = (JObject)_data;
        }

        #endregion Constructors

        #region Properties

        public string id
        {
            get { return data["ID"].ToString(); }
        }

        public bool IsInDetail
        {
            get
            {
                return data.Count > 25;
            }
        }

        public bool IsJob
        {
            get { return id.StartsWith('j'); }
        }

        public bool IsProject
        {
            get { return id.StartsWith('p'); }
        }

        #endregion Properties

        #region Methods

        public async Task<JObject> LoadDetail(ClientConnection client)
        {
            if (!IsInDetail)
            {
                if (IsJob)
                    return await Requests.Jobs.GetJobData(client, id.Replace("j", ""));
#warning only supports loading of job detail
            }
            return data;
        }

        #endregion Methods
    }
}