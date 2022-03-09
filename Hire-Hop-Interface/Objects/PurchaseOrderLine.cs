using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Hire_Hop_Interface.Objects
{
    public class PurchaseOrderLine : JsonObject
    {
        #region Properties

        public int ACC_NOMINAL_ID
        {
            get { return json.Value.TryGetProperty("ACC_NOMINAL_ID", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int ACC_TAX_RATE_ID
        {
            get { return json.Value.TryGetProperty("ACC_TAX_RATE_ID", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public string DESCRIPTION
        {
            get { return json.Value.TryGetProperty("DESCRIPTION", out JsonElement e) ? e.GetString() : ""; }
        }

        public int ID
        {
            get { return json.Value.TryGetProperty("ID", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int INTERNAL_ITEM
        {
            get { return json.Value.TryGetProperty("INTERNAL_ITEM", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public double internal_total
        {
            get { return json.Value.TryGetProperty("internal_total", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public int LFT
        {
            get { return json.Value.TryGetProperty("LFT", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int MAIN_ID
        {
            get { return json.Value.TryGetProperty("MAIN_ID", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public string PART_NUMBER
        {
            get { return json.Value.TryGetProperty("PART_NUMBER", out JsonElement e) ? e.GetString() : ""; }
        }

        public double PRICE
        {
            get { return json.Value.TryGetProperty("PRICE", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public double QTY
        {
            get { return json.Value.TryGetProperty("QTY", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public int RESOURCE
        {
            get { return json.Value.TryGetProperty("RESOURCE", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int RGT
        {
            get { return json.Value.TryGetProperty("RGT", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public double subs_total
        {
            get { return json.Value.TryGetProperty("subs_total", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public int TYPE
        {
            get { return json.Value.TryGetProperty("TYPE", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public double UNIT_PRICE
        {
            get { return json.Value.TryGetProperty("UNIT_PRICE", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public int VAT
        {
            get { return json.Value.TryGetProperty("VAT", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public double VAT_RATE
        {
            get { return json.Value.TryGetProperty("VAT_RATE", out JsonElement e) ? e.GetDouble() : -1; }
        }

        public int VIRTUAL
        {
            get { return json.Value.TryGetProperty("VIRTUAL", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public double WEIGHT
        {
            get { return json.Value.TryGetProperty("WEIGHT", out JsonElement e) ? e.GetDouble() : -1; }
        }

        #endregion Properties
    }
}