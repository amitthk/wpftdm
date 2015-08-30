using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Data
{
    public class TodoDatabase
    {
        public static IDocumentStore _instance;

        public static IDocumentStore Instance
        {
            get
            {
                if (_instance!=null)
                {
                    return (_instance);
                }

                lock (typeof(TodoDatabase))
                {
                    if (_instance!=null)
                    {
                        return (_instance);
                    }

                    _instance = new EmbeddableDocumentStore
                    {
                        DataDirectory = AppSettings.Instance.AppDataPath,
                        UseEmbeddedHttpServer=false
                    };
                    return (_instance);
                }
            }
        }
    }    
}
