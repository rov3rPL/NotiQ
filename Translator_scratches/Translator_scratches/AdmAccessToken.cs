using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Translator_scratches
{
    public class AdmAccessToken
    {

        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string scope { get; set; }
    }

    
}