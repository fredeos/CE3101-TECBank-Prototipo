using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace tecbank.DBMS{
    /// <summary>
    /// Accesspoint (AP) for a table in a database
    /// </summary>
    public class TableAP{
        // --------------------------------[ Class atributes ]--------------------------------
        private String __table_file;
        public int? __table_id;
        public String? __table_name;
        private List<String> primary_keys = [];
        private List<String> foreign_keys = [];
        // --------------------------------[ Class methods ]--------------------------------
        public TableAP(String table){
            this.__table_file = table;
            // >> Cargar las propiedades de la tabla
            var xml_doc = XDocument.Load(__table_file);
            if (xml_doc == null) throw new SystemException($"(TableAP) The XML table file doesn't exist: {__table_file}");

            this.__table_name = xml_doc.Element("table").Element("name")?.Value;
            this.__table_id = int.Parse(xml_doc.Element("table").Element("id")?.Value);
            foreach (var pk in xml_doc.Descendants("PK")){
                primary_keys.Add(pk.Value);
            }
            foreach (var fk in xml_doc.Descendants("FK")){
                foreign_keys.Add(fk.Value);
            }
        }

        /// <summary>
        /// Finds all the tuples in a table with the matching criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<T> find<T> (Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    using (StringReader reader = new StringReader(value.ToString())){
                        T tup = (T) serializer.Deserialize(reader);
                        if (criteria.Invoke(tup)){
                            tuples.Add(tup);
                        }
                    }
                }
            }
            return tuples;
        }

        /// <summary>
        /// Extracts all elements from the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> extract_all<T>(){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    using (StringReader reader = new StringReader(value.ToString())){
                        T tup = (T) serializer.Deserialize(reader);
                        tuples.Add(tup);
                    }
                }
            }
            return tuples;
        }

        /// <summary>
        /// Serializes a model object to XElement, it must have the proper XML tags for its attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static XElement? SerializeToXElement<T>(T obj){
            var mem_stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(mem_stream,obj);
            mem_stream.Position = 0;
            XmlReader reader = XmlReader.Create(mem_stream);
            return XElement.Load(reader);
        }

        /// <summary>
        /// Verifies two tuples are not the same to prevent data integrity problems in the table
        /// </summary>
        /// <param name="target1">XElement object</param>
        /// <param name="target2">XElement object</param>
        /// <returns>true if target1 is different from target2, otherwise false</returns>
        private bool TableIntegrityCheck(XElement target1, XElement target2){
            foreach(var PK in primary_keys){
                Console.WriteLine($"{target1.Element(PK).Value}:{target2.Element(PK).Value}");
                if(target1.Element(PK).Value == target2.Element(PK).Value){
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Modifies all objects that match with the criteria with the content of the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="criteria"></param>
        public void modify<T>(T obj, Func<T,T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach(var value in values.ToList()){
                    using (StringReader reader = new StringReader(value.ToString())){
                        T tuple = (T) serializer.Deserialize(reader);
                        if (criteria.Invoke(tuple,obj)){
                            value.ReplaceWith(SerializeToXElement<T>(obj));
                        }
                    }
                }
            }
            xml_doc.Save(__table_file);
        }

        /// <summary>
        /// Create a new tuple for the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void create<T>(T obj){
            XDocument xml_doc = XDocument.Load(__table_file);
            var tuples = xml_doc.Descendants("tuples").FirstOrDefault();
            var values = xml_doc.Descendants("value");
            if (tuples != null && values != null){
                bool exists = false;
                foreach(var val in values.ToList()){
                    if (!this.TableIntegrityCheck(val,SerializeToXElement<T>(obj))){
                        exists = true;
                        break;
                    }
                }
                
                if (!exists){
                    tuples.Add(SerializeToXElement<T>(obj));
                } else {
                    throw new ArgumentException($"(TableAP) {typeof(T)} obj already exists in the table {__table_name}:{__table_id}");
                }
            }
            
            xml_doc.Save(__table_file);
        }

        /// <summary>
        /// Remove all tuples of the table that match the given criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        public void delete<T>(Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var tuples = xml_doc.Descendants("tuples").FirstOrDefault();
            if (tuples != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in tuples.Elements().ToList()){
                    using (StringReader reader = new StringReader(value.ToString())){
                        T tuple = (T) serializer.Deserialize(reader);
                        if (criteria.Invoke(tuple)){
                            value.Remove();
                        }
                    }
                }
            } 
            xml_doc.Save(__table_file);
        }
    }
}