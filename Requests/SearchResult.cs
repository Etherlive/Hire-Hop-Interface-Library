using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;

namespace Hire_Hop_Interface.Requests
{
    public class SearchResult
    {
        #region Fields

        private JObject data;

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

        public async void LoadInDetail(ClientConnection client)
        {
            if (!IsInDetail)
            {
                if (IsJob)
                    data = await Jobs.GetJobData(client, id.Replace("j", ""));
                else if (IsProject)
#warning only supports loading of job detail
                    data = data;
            }
        }

        #endregion Methods
    }
}