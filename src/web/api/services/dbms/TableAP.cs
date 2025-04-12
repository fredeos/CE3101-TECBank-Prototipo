using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace tecbank.DBMS{
    /// <summary>
    /// Accesspoint (AP) for a table in a database. <T> objects used in class methods must have valid Xml format attributes for correct mapping
    /// </summary>
    public class TableAP{
        // --------------------------------[ Class atributes ]--------------------------------
        private String __table_file;
        public int? __table_id;
        public String? __table_name;
        private int? __remove_type;
        private List<String> primary_keys = [];
        private List<String> foreign_keys = [];
        // --------------------------------[ Class methods ]--------------------------------
        public TableAP(String table){
            this.__table_file = table;
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentException("(TableAP) The name of the table is invalid");

            // >> Cargar las propiedades de la tabla
            var xml_doc = XDocument.Load(__table_file);
            if (xml_doc == null) 
                throw new XmlException($"(TableAP) The XML table file doesn't exist: {__table_file}");

            // >> Cargar propiedades de la tabla
            var table_xml = xml_doc.Element("table") ?? 
                throw new SystemException($"(TableAP) The database table file {__table_file} doesn't have a valid 'table' element");

            // Manejo seguro del nombre de tabla
            this.__table_name = table_xml.Element("name")?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(__table_name))
                throw new SystemException($"(TableAP) The table doesn't have a valid name property: {__table_file}");

            // Manejo seguro del ID de tabla
            var idElement = table_xml.Element("id");
            if (idElement == null || !int.TryParse(idElement.Value, out var tableId))
                throw new SystemException($"(TableAP) The table doesn't have a valid ID property: {__table_file}");
            this.__table_id = tableId;

            // Manejo seguro del tipo de removido para la tabla
            var removal = table_xml.Element("removal")?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(removal))
                throw new SystemException($"(TableAP) The table doesn't have a valid removal type property: {__table_file}");
            if (removal == "logical"){
                this.__remove_type = 1;
            } else if (removal == "physical"){
                this.__remove_type = 0;
            } else {
                this.__remove_type = -1;
            }
            

            // >> Cargar llaves de la tabala
            foreach (var pk in xml_doc.Descendants("PK")){
                primary_keys.Add(pk.Value);
            }
            foreach (var fk in xml_doc.Descendants("FK")){
                foreign_keys.Add(fk.Value);
            }
        }

        /// <summary>
        /// Finds all ocurrences of tuples that match the given criteria
        /// </summary>
        /// <returns>List of matching T tuples</returns>
        /// <exception cref="XmlException"></exception>
        public List<T> find<T> (Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    try{
                        using (StringReader reader = new StringReader(value.ToString())){
                            T? tup = (T)serializer.Deserialize(reader);
                            if (tup!=null && criteria.Invoke(tup)){
                                tuples.Add(tup);
                            }
                        }   
                    } catch (System.Exception e1){
                        throw new XmlException($"(TableAP) Serialization of table value failed:{value.ToString()}\n{e1.ToString()}");
                    }
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <value> elements");
            }
            return tuples;
        }

        /// <summary>
        /// Extracts all tuples from the table
        /// </summary>
        /// <returns>List of T elements</returns>
        /// <exception cref="XmlException"></exception>
        public List<T> extract_all<T>(){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    try{
                        using (StringReader reader = new StringReader(value.ToString())){
                            T tup = (T) serializer.Deserialize(reader);
                            if (tup != null)
                                tuples.Add(tup);
                        }   
                    } catch (System.Exception e1){
                        throw new XmlException($"(TableAP) Serialization of table value failed:{value.ToString()}\n{e1.ToString()}");
                    }
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <value> elements");
            }
            return tuples;
        }

        /// <summary>
        /// Serializes a model object to XElement, it must have the proper XML tags for its attributes
        /// </summary>
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
        /// <returns>"true" if target1 is different from target2, otherwise "false"</returns>
        private bool TableIntegrityCheck(XElement target1, XElement target2){
            foreach(var PK in primary_keys){
                if(target1.Element(PK).Value == target2.Element(PK).Value){
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Modifies all objects that match the criteria with the content of the given object
        /// </summary>
        public void modify<T>(T obj, Func<T,T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach(var value in values.ToList()){
                    using (StringReader reader = new StringReader(value.ToString())){
                        T tuple = (T) serializer.Deserialize(reader);
                        if (tuple != null && criteria.Invoke(tuple,obj)){
                            value.ReplaceWith(SerializeToXElement<T>(obj));
                        }
                    }
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <value> elements");
            }
            xml_doc.Save(__table_file);
        }

        /// <summary>
        /// Creates a new entity tuple in the table
        /// </summary>
        /// <exception cref="XmlException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void create<T>(T obj){
            XDocument xml_doc = XDocument.Load(__table_file);
            var tuples = xml_doc.Descendants("tuples").FirstOrDefault();
            var values = xml_doc.Descendants("value");
            if (tuples != null && values != null){
                // >> Serializacion del objeto a T a un elemento xml
                var XObj = SerializeToXElement<T>(obj) ??
                    throw new XmlException($"(TableAP) {nameof(T)} couldn't be serialized to a valid XElement object");

                // >> Verificacion de duplicidad de valores
                bool exists = false;
                foreach(var val in values.ToList()){
                    if (!this.TableIntegrityCheck(val,XObj)){
                        exists = true;
                        break;
                    }
                }
                
                if (!exists){
                    tuples.Add(XObj);
                } else {
                    throw new ArgumentException($"(TableAP) {nameof(T)} object already exists in the table {__table_name}:{__table_id}");
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <tuples> and/or <value> elements");
            }
            xml_doc.Save(__table_file);
        }

        /// <summary>
        /// Deletes all ocurrences that match the criteria.
        /// It does logic removals if the XML property specifies it, else does a physical remove from the table file
        /// </summary>
        /// <exception cref="XmlException"></exception>
        public void delete<T>(Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var tuples = xml_doc.Descendants("tuples").FirstOrDefault();
            if (tuples != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in tuples.Elements().ToList()){
                    try{
                        using (StringReader reader = new StringReader(value.ToString())){
                            T tuple = (T) serializer.Deserialize(reader);
                            if (tuple!=null && criteria.Invoke(tuple)){
                                if (this.__remove_type == 0){ // Physical remove
                                    value.Remove();
                                } else if (this.__remove_type == 1){ // Logical remove
                                    value.Element("rem_state").SetValue(1);
                                }
                            }
                        }
                    } catch (System.Exception e1){
                        throw new XmlException($"(TableAp) Serialization of table value failed: {value.ToString()}\n{e1.ToString()}");
                    }
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <tuples> element");
            }
            xml_doc.Save(__table_file);
        }

        /// <summary>
        /// Removes all ocurrences that match criteria from file regardless of specified removal type
        /// </summary>
        /// <exception cref="XmlException"></exception>
        public void true_delete<T>(Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var tuples = xml_doc.Descendants("tuples").FirstOrDefault();
            if (tuples != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in tuples.Elements().ToList()){
                    try{
                        using (StringReader reader = new StringReader(value.ToString())){
                            T tuple = (T) serializer.Deserialize(reader);
                            if (tuple!=null && criteria.Invoke(tuple)){
                                value.Remove();
                            }
                        }   
                    } catch (System.Exception e1){
                        throw new XmlException($"(TableAp) Serialization of table value failed: {value.ToString()}\n{e1.ToString()}");
                    }
                }
            } else {
                throw new XmlException($"(TableAP) Database table file {__table_file} doesn't contain expected <tuples> element");
            }
            xml_doc.Save(__table_file);
        }
    }
}