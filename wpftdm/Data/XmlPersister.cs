using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace wpftdm.Data
{
    public class XmlPersister<T>
    {
        private string _dataFileName;
        private List<T> _objList;

        public List<T> objList
        {
            get { return (_objList); }
        }

        public XmlPersister(string dataFileName)
        {
            // Sets the location of datafile into App_Data Directory of our Application 
            //(We can change the location of our datafiles to Other directory as well)
            // This constructor creates a Persister of supplied type
            //
            this._dataFileName = System.IO.Path.Combine(AppConstants.AppBasePath,
                     dataFileName);
            this._objList = Activator.CreateInstance<List<T>>();
        }

        public void save()
        {
            lock (_dataFileName)
            {
                using (FileStream writer = File.Create(_dataFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(_objList.GetType());
                    serializer.Serialize(writer, _objList);
                }
            }
        }

        public void load()
        {
            if (File.Exists(_dataFileName))
            {
                lock (_dataFileName)
                {
                    using (FileStream reader = File.Open(_dataFileName,
                             FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                        _objList = (List<T>)serializer.Deserialize(reader);
                    }
                }
            }
            else
            {
                _objList = new List<T>();
                save();
            }
        }
    }
}
