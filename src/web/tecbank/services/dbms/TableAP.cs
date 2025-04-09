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
            Console.WriteLine($"Leyendo tabla: {__table_file}");

            // >> Cargar las propiedades de la tabla
            var xml_doc = XDocument.Load(__table_file);
            if (xml_doc == null) throw new SystemException($"El archivo de tabla {__table_file} no existe");

            this.__table_name = xml_doc.Element("table").Element("name")?.Value;
            this.__table_id = int.Parse(xml_doc.Element("table").Element("id")?.Value);
            foreach (var pk in xml_doc.Elements("PKs").Descendants("PK")){
                primary_keys.Add(pk.Value);
            }
            foreach (var fk in xml_doc.Elements("FKs").Descendants("FK")){
                foreign_keys.Add(fk.Value);
            }
        }

        public List<T> find<T> (Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    Console.WriteLine(value.ToString());
                    StringReader reader = new StringReader(value.ToString());
                    T tup = (T) serializer.Deserialize(reader);
                    reader.Close();
                    tuples.Add(tup);
                }
            } else {
                return [];
            }
            return tuples.Where(criteria).ToList();
        }

        //
        public List<T> extract_all<T>()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                List<T> results = new List<T>();
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                // Buscar todos los nodos value, independientemente de su profundidad
                var valueNodes = xmlDoc.Descendants("value").ToList();

                foreach (var valueNode in valueNodes)
                {
                    try
                    {
                        XElement nodeToProcess = valueNode;

                        // Si el nodo value contiene otro nodo value, usamos el interno
                        if (valueNode.Elements("value").Any())
                        {
                            nodeToProcess = valueNode.Element("value");
                        }

                        using (StringReader reader = new StringReader(nodeToProcess.ToString()))
                        {
                            T obj = (T)serializer.Deserialize(reader);
                            if (obj != null)
                            {
                                results.Add(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Loggear el error y continuar
                        Console.WriteLine($"Error deserializando nodo: {ex.Message}");
                        continue;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al leer registros: {ex.Message}");
            }
        }

        public void modify<T>(T obj, Func<T,bool> criteria){

        }

        public void create<T>(T obj)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                XElement root = xmlDoc.Element("table");
                
                // Asegurar que existe el elemento tuples
                var tuples = root.Element("tuples") ?? new XElement("tuples");
                if (tuples.Parent == null)
                {
                    root.Add(tuples);
                }
                
                // Serializar directamente al formato deseado
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Eliminar namespaces
                
                // Crear el elemento value directamente con el contenido serializado
                XElement newValue;
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj, ns);
                    // Convertir el XML serializado a XElement
                    newValue = XElement.Parse(writer.ToString());
                }
                
                // Renombrar el elemento raíz del objeto a "value"
                newValue.Name = "value";
                
                tuples.Add(newValue);
                xmlDoc.Save(__table_file);
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al crear registro: {ex.Message}");
            }
        }

        public void delete<T>(T obj, Func<T, bool> criteria){

        }
    }
}