using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Reflection;
using General.Database.Common;
using General.Common;
using General.Common.Utils;
using People.Browser.Common;
using AuditSeverity = People.Browser.Common.AuditSeverity;
using Constants = General.Common.Constants;

namespace People.Browser.DAL
{
    public class MongoDbDal : IRepositoryDatabase
    {
        #region Events
        
        public event AuditMessage Message;
        public event EventHandler RepositoryDatabaseMessage;

        #endregion

        #region Data Members

        private MongoClient _client;

        private IMongoDatabase _database;

        private string[] _collection; 

        #endregion

        #region Properties
        
        /// <summary>
        /// Port of the Mongo DB default is 27017
        /// </summary>
        public string MongoPort { get; set; }

        /// <summary>
        /// IP of the mongo DB. Default is localhost
        /// </summary>
        public string MongoIp { get; set; }

        public string DatabaseName { get; set; }

        #endregion

        #region Database

        //  Purpose:        Mongo database DAL CTOR
        //  Input:          none
        //  Output:         none
        public MongoDbDal()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            //MongoIp = "localhost";
            //MongoPort = "27017";
        }

        //  Purpose:        Open database
        //  Input:          databaseParameters - see ContextRepositoryDataTypes.OpenDatabaseParameters in ContextRepository.Commom\Data Types.cs
        //                  result - [out] string
        //  Output:         true / false
        public bool OpenDb(OpenDatabaseParameters databaseParameters, out string result)
        {
            string report = string.Empty;
            string user_password = string.Empty;

            result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(databaseParameters.DatabaseUsername))
                {
                    user_password = databaseParameters.DatabaseUsername + ":" + databaseParameters.DatabasePassword + "@";
                }

                MongoIp = databaseParameters.DatabaseIpAddress;
                MongoPort = databaseParameters.DatabaseIpPort;

                _client = new MongoClient(string.Format("mongodb://" + user_password + "{0}:{1}", MongoIp, MongoPort));
                DatabaseName = databaseParameters.DatabaseName;
                _database = _client.GetDatabase(databaseParameters.DatabaseName);

                // Remove the deserialize exception - without it when try to get an object from the mongo,
                // it will complain that we have no field named _id in the class
                var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
                ConventionRegistry.Register("IgnoreExtraElementsConvention", pack, t => true);
                if (IsConnected(out result))
                {
                    result = "Connect - Connected to MongoDB IP[" + MongoIp + "] Port[" + MongoPort + "] Database:[" + databaseParameters.DatabaseName + "]";
                    report += result;
                }
                else
                {
                    result = "Failed Connecting to MongoDB IP[" + MongoIp + "] Port[" + MongoPort + "] Database:[" + databaseParameters.DatabaseName + "]";

                    string yesNo = (string.IsNullOrEmpty(user_password)) ? "No " : string.Empty;
                    
                    result += Environment.NewLine + yesNo + "User/Password Used To Authenticate";                        

                    return false;
                }

                _collection = StringUtils.GetAllSubStrings(databaseParameters.DatabaseTables, ":");

                if (_collection != null)
                {
                    //  get all needed tables/collections
                    foreach (string currentCollection in _collection)
                    {
                        //  if table/collection exists do nothing
                        if (CollectionExist(currentCollection, out result))
                        {
                            report += Environment.NewLine + "Collection[" + currentCollection + "] exists";
                            continue;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(result))
                            {
                                report += Environment.NewLine + result;
                            }
                        }

                        //  if table/collection does not exist create it
                        CreateCollection(currentCollection, out result);

                        report += Environment.NewLine + result;
                    }
                }
                else
                {
                    report += Environment.NewLine + "No collections in list";
                    result = report;

                    return false;
                }

                result = report;

                return true;
            }
            catch (Exception e)
            {
                result = report + Environment.NewLine + e.Message;

                return false;
            }
        }

        //  Purpose:        Is Mongo database connected
        //  Input:          result - out result
        //  Output:         true / false
        public bool IsConnected(out string result)
        {
            try
            {
                if (_database != null)
                {
                    bool pingResult = _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(5000);

                    if (pingResult)
                    {
                        result = "Connected";
                    }
                    else
                    {
                        result = "Could Not Connect";
                    }
                    //return pingResult.ElementCount == 1 && pingResult.Elements.First().Name == "ok";
                    return pingResult;
                }

                result = "Database Object Is Null";

                return false;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Close Mongo database 
        //  Input:          result - [out] result
        //  Output:         true / false
        public bool CloseDb(out string result)
        {
            result = string.Empty;

            try
            {
                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        } 

        #endregion

        #region Actions

        //  Purpose:        Insert documents, one or more into collection 'collectionName'
        //  Input:          collectionName 
        //                  documentFields
        //                  [out] newId
        //                  result - [out] result
        //  Output:         true / false
        public bool Insert(string collectionName, Dictionary<string, string> documentFields, out string newId, out string result)
        {
            result = string.Empty;
            newId = string.Empty;

            try
            {
                BsonDocument _bsonDocument = new BsonDocument(documentFields);

                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertOne(_bsonDocument);

                newId =_bsonDocument.GetElement("_id").Value.ToString();

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Insert documents, one or more into collection 'collectionName'
        //  Input:          collectionName 
        //                  documentFields
        //                  documentFieldTypes
        //                  [out] newId
        //                  result - [out] result
        //  Output:         true / false
        public bool Insert(string collectionName, Dictionary<string, string> documentFields, Dictionary<string, Type> documentFieldTypes, out string newId, out string result)
        {
            result = string.Empty;
            newId = string.Empty;

            try
            {
                if ((documentFields == null) && (documentFields.Count == 0))
                {
                    result = "Fields List Is Emty";

                    return false;
                }

                if ((documentFieldTypes == null) && (documentFieldTypes.Count == 0))
                {
                    result = "Field Types List Is Emty";

                    return false;
                }

                if (documentFields.Count != documentFieldTypes.Count)
                {
                    result = "Fields List And Field Types List Are Not In The Same Size";

                    return false;
                }

                BsonDocument _bsonDocument = new BsonDocument();

                for (int i = 0; i < documentFields.Count; i++)
                {
                    string key;
                    string value;
                    Type type;

                    key = documentFields.ElementAt(i).Key;
                    value = documentFields.ElementAt(i).Value;
                    type = documentFieldTypes.ElementAt(i).Value;

                    if (type == typeof(int))
                    {
                        if (!int.TryParse(value, out int iValue))
                        {
                            result = "'" + value + "' Is Not An Integer";

                            return false;
                        }

                        _bsonDocument.Add(new BsonElement(key, new BsonInt32(iValue)));
                        continue;
                    }

                    if (type == typeof(DateTime))
                    {
                        CultureInfo cultureInfo = new CultureInfo("en-US");

                        if (!DateTime.TryParseExact(value, Constants.DATE_TIME_FORMAT, cultureInfo, DateTimeStyles.AssumeLocal, out DateTime dtValue))
                        {
                            result = "'" + value + "' Is Not A DateTime Or In Wrong Format. Should Be 'dd-MM-yyyy HH:mm:ss.fff'";

                            return false;
                        }

                        _bsonDocument.Add(new BsonElement(key, new BsonDateTime(dtValue)));
                        continue;
                    }

                    if (type == typeof(string))
                    {
                        _bsonDocument.Add(new BsonElement(key, new BsonString(value)));
                        continue;
                    }

                    result = "Type '" + type.ToString() + "' Not Supported";

                    return false;
                }

                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertOne(_bsonDocument);

                newId = _bsonDocument.GetElement("_id").Value.ToString();

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Update document with new value to 'targetField' field by key field 'keyField' in collection 'collectionName'
        //  Input:          collectionName 
        //                  keyField
        //                  keyFieldType
        //                  keyFieldValue
        //                  targetField
        //                  targetFieldType
        //                  targetFieldNewValue
        //                  result - [out] result
        //  Output:         true / false
        public bool Update(string collectionName, 
                           string keyField, 
                           Type keyFieldType, 
                           string keyFieldValue, 
                           string targetField, 
                           Type targetFieldType,  
                           string targetFieldNewValue, 
                           out string result)
        {
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                if (targetFieldType == typeof(int))
                {
                    if (!int.TryParse(targetFieldNewValue, out int iValue))
                    {
                        result = "'" + targetFieldNewValue + "' Is Not An Integer";

                        return false;
                    }

                    collection.FindOneAndUpdateAsync(Builders<BsonDocument>.Filter.Eq(keyField, keyFieldValue),
                                                     Builders<BsonDocument>.Update.Set(targetField, iValue));

                    return true;
                }

                if (targetFieldType == typeof(DateTime))
                {
                    CultureInfo cultureInfo = new CultureInfo("en-US");

                    if (!DateTime.TryParseExact(targetFieldNewValue, Constants.DATE_TIME_FORMAT, cultureInfo, DateTimeStyles.AssumeLocal, out DateTime dtValue))
                    {
                        result = "'" + targetFieldNewValue + "' Is Not A DateTime Or In Wrong Format. Should Be 'dd-MM-yyyy HH:mm:ss.fff'";

                        return false;
                    }

                    collection.FindOneAndUpdateAsync(Builders<BsonDocument>.Filter.Eq(keyField, keyFieldValue),
                                                     Builders<BsonDocument>.Update.Set(targetField, dtValue));                   

                    return true;
                }

                if (targetFieldType == typeof(string))
                {
                    collection.FindOneAndUpdateAsync(Builders<BsonDocument>.Filter.Eq(keyField, keyFieldValue),
                                                     Builders<BsonDocument>.Update.Set(targetField, targetFieldNewValue));

                    return true;
                }

                result = "Type '" + targetFieldType.ToString() + "' Not Supported";

                return false;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Retrieve documents, one or more into collection 'collectionName'
        //  Input:          collectionName 
        //                  filter - search criteria
        //                  records - the fetched documents
        //                  result - [out] result
        //  Output:         true / false
        public bool Retrieve(string collection, Dictionary<string, string> filter, out List<List<string>> records, out string result)
        {
            records = null;

            try
            {
                if (!TryGetAllDocuments(collection, filter, out records, out result))
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Document with 'field' == 'value' exists in the collection 'collectionName'
        //  Input:          collectionName 
        //                  field - criteria
        //                  fieldType
        //                  value - the value of 'field'
        //                  result - [out] result
        //  Output:         true / false
        public bool RecordExists(string collection, string field, Type fieldType, string value, out string result)
        {
            BsonDocument filter;

            try
            {
                filter = new BsonDocument(field, value);

                return (DocumentExists(collection, filter, out result));
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Remove document with 'field' == 'value' from collection 'collectionName'
        //  Input:          collectionName 
        //                  field - criteria
        //                  fieldType
        //                  value - the value of 'field'
        //                  result - [out] result
        //  Output:         true / false
        public bool Remove(string collection, string field, Type fieldType, string value, out string result)
        {
            try
            {
                if (value == "{}")
                {
                    return DeleteAllCollectionDocuments(collection, out result);
                }
                else
                {
                    return RemoveDocument(collection, field, value, out result);
                }
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        Sum all 'accumulationField' where 'idField' == 'value' from collection 'collectionName'
        //  Input:          collectionName 
        //                  idField - criteria
        //                  accumulationField
        //                  value - the value of 'idField'
        //                  result - [out] result
        //  Output:         sum result
        public int Sum(string collectionName, string idField, Type fieldType, string value, string accumulationField, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            int sum = 0;
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                FilterDefinition<BsonDocument> filter = new BsonDocument(idField, value);

                var aggregate = collection.Aggregate()
                                           .Match(filter);

                List<BsonDocument> results = aggregate.ToList();

                foreach (var item in results)
                {
                    sum += int.Parse(item.GetValue(accumulationField).AsString);
                }

                return sum;
            }
            catch (Exception e)
            {
                result = "Failed sum '" + accumulationField + "' for '" + idField + "' = " + value + " from collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return Constants.NONE;
            }
        }

        //  Purpose:        Count number of documents where 'idField' == 'value' in collection 'collectionName'
        //  Input:          collectionName 
        //                  idField - criteria
        //                  value - the value of 'idField'
        //                  result - [out] result
        //  Output:         sum result
        public int Count(string collectionName, string idField, Type fieldType, string value, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            int sum = 0;
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                FilterDefinition<BsonDocument> filter = new BsonDocument(idField, value);

                var aggregate = collection.Aggregate()
                                           .Match(filter);

                List<BsonDocument> results = aggregate.ToList();

                foreach (var item in results)
                {
                    sum += 1;
                }

                return sum;
            }
            catch (Exception e)
            {
                result = "Failed count number of appearances of '" + idField + "' = " + value + " from collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return Constants.NONE;
            }
        }

        //  Purpose:        Distinct values of 'idField' from collection 'collectionName'
        //  Input:          collectionName 
        //                  idField 
        //                  [out] list of distinct 'idField' values    
        //                  result - [out] result
        //  Output:         true / false
        //  Assumptions:    none
        public bool Distinct(string collectionName, string idField, out List<string> distictList, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            List<string> innerDistictList = new List<string>();

            result = string.Empty;
            distictList = null;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                BsonDocument filter = new BsonDocument();

                var distinctList = collection.Distinct<string>(idField, filter).ToList();

                distinctList.ForEach(x => innerDistictList.Add(x));

                distictList = innerDistictList;

                return true;
            }
            catch (Exception e)
            {
                result = "Failed DISTINCT for '" + idField + "' from collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        //  Purpose:        Get wanted fields values by field,value pairs as criteria from collection 'collectionName'
        //  Input:          collectionName 
        //                  fieldValue - criteria field,value pairs
        //                  wantedField - the fields to be retrieved
        //                  result - [out] result
        //  Output:         list of wanted fields values
        public List<string> GetBy(string collectionName, Dictionary<string, string> fieldValue, List<string> wantedField, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            List<string> lValue = new List<string>();
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                var builder = Builders<BsonDocument>.Filter;
                FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;

                //if ((fieldValue != null) && (fieldValue.Count > 0))
                //{
                //    filter = fieldValue.Aggregate(filter, (current, pair) => current & builder.Eq(pair.Key, pair.Value));
                //}

                if ((fieldValue != null) && (fieldValue.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pair in fieldValue)
                    {
                        filter &= builder.Eq(pair.Key, pair.Value);
                    }
                }

                var aggregate = collection.Aggregate()
                                          .Match(filter);

                List<BsonDocument> results = aggregate.ToList();

                foreach (var item in results)
                {
                    if (wantedField != null)
                    {
                        for (int i = 0; i < wantedField.Count; i++)
                        {
                            //lValue.Add(item.GetValue(wantedField[i]).AsString);
                            lValue.Add(item.GetElement(wantedField[i]).Value.ToString());
                        }
                    }
                }

                return lValue;
            }
            catch (Exception e)
            {
                result = "Failed getting wanted fields {} by these fields and values from collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return null;
            }
        }

        //  Purpose:        Get record from collection 'collectionName' where 'fieldName' is maximal 
        //  Input:          collectionName 
        //                  documentFields
        //                  fieldName 
        //                  record - [out] the retrieved record
        //                  result - [out] result
        //  Output:         true / false
        public bool Max(string collectionName, List<string> documentFields, string fieldName, out List<string> record, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            record = new List<string>();
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                var aggregate = collection.Aggregate().Sort("{" + fieldName + ": -1}");

                List<BsonDocument> results = aggregate.ToList();

                if ((results == null) || (results.Count == 0))
                {
                    result = "Results List Is Empty";

                    Audit($"{result}", method, LINE(), AuditSeverity.Warning);

                    return false;
                }

                if ((documentFields != null) && (documentFields.Count > 0))
                {
                    foreach (string key in documentFields)
                    {
                        record.Add(results[0].GetElement(key).Value.ToString());
                    }
                }
                else
                {
                    result = "No Fields Defined For Document";

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                result = "Failed Finding Record For Maximum Of Field '" + fieldName + "' From Collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        //  Purpose:        Get record from collection 'collectionName' where 'fieldName' is minimal 
        //  Input:          collectionName 
        //                  documentFields
        //                  fieldName 
        //                  record - [out] the retrieved record
        //                  result - [out] result
        //  Output:         true / false
        public bool Min(string collectionName, List<string> documentFields, string fieldName, out List<string> record, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            record = new List<string>();
            result = string.Empty;

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                var aggregate = collection.Aggregate().Sort("{" + fieldName + ": 1}");

                List<BsonDocument> results = aggregate.ToList();

                if ((results == null) || (results.Count == 0))
                {
                    result = "Results List Is Empty";

                    Audit($"{result}", method, LINE(), AuditSeverity.Warning);

                    return false;
                }

                if ((documentFields != null) && (documentFields.Count > 0))
                {
                    foreach (string key in documentFields)
                    {
                        record.Add(results[0].GetElement(key).Value.ToString());
                    }
                }
                else
                {
                    result = "No Fields Defined For Document";

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                result = "Failed Finding Record For Minimum Of Field '" + fieldName + "' From Collection [" + collectionName + "]." + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        } 

        #endregion

        #region Collections
        
        //  Purpose:        Collection 'collectionName' exists?
        //  Input:          collectionName 
        //                  result - [out] result
        //  Output:         true / false
        //  Assumptions:    none
        private bool CollectionExist(string collectionName, out string result)
        {
            result = string.Empty;

            try
            {
                List<string> lCollections = _database.ListCollectionNames().ToList();
                int index = lCollections.IndexOf(collectionName);

                if (index >= 0)
                {
                    return true;
                }

                result = "Collection[" + collectionName + "] does not exist";

                return false;
            }
            catch (Exception e)
            {
                result = "Could not determine if collection[" + collectionName + "] exists. " + e.Message;

                return false;
            }
        }

        //  Purpose:        Create collection 'collectionName'
        //  Input:          collectionName 
        //                  result - [out] result
        //  Output:         true / false
        private bool CreateCollection(string collectionName, out string result)
        {
            try
            {
                if (CollectionExist(collectionName, out result))
                {
                    result = "Collection with name:[" + collectionName + "] already exists. ";

                    return true;
                }
                else
                {
                    /* false to disable the automatic creation of an index on the _id field */
                    var createCollectionOptions = new CreateCollectionOptions();
                    _database.CreateCollection(collectionName, createCollectionOptions);

                    result = "Creating a new collection with the name:[" + collectionName + "]";

                    return true;
                }
            }
            catch (Exception e)
            {
                result = "Creating a new collection with the name:[" + collectionName + "] failed. " + e.Message;

                return false;
            }
        } 

        #endregion

        #region Documents

        //  Purpose:        Delete all documents of collection 'collectionName'
        //  Input:          collectionName 
        //                  result - [out] result
        //  Output:         true / false
        private bool DeleteAllCollectionDocuments(string collectionName, out string result)
        {
            result = string.Empty;
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.DeleteMany(_ => true);

                return true;
            }
            catch (Exception e)
            {
                result = "Creating a new collection with the name:[" + collectionName + "] failed. " + e.Message;

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        //  Purpose:        Remove document by 'field' == 'value' of collection 'collectionName'
        //  Input:          collectionName 
        //                  field
        //                  value
        //                  result - [out] result
        //  Output:         true / false
        private bool RemoveDocument(string collectionName, string field, string value, out string result)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            BsonDocument filter;

            result = string.Empty;

            var collection = _database.GetCollection<BsonDocument>(collectionName);
            if (null == collection)
            {
                result = "Collection '" + collectionName + "' doesn't exist.";

                Audit($"{result}", method, LINE(), AuditSeverity.Error);

                return false;
            }
            try
            {
                filter = new BsonDocument(field, value);
                collection.DeleteOne(filter);

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                Audit($"An Excepion Occurd While Deleting A Document In Collection '{collectionName}'. {e.Message}", 
                      method, 
                      LINE(), 
                      AuditSeverity.Error);

                return false;
            }
        }

        //  Purpose:        Add document by to collection 'collectionName'
        //  Input:          collectionName 
        //                  context
        //                  result - [out] result
        //  Output:         true / false
        private bool AddDocumentToCollection(string collectionName, BsonDocument context, out string result)
        {
            result = string.Empty;

            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                collection.InsertOneAsync(context);

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                Audit($"Failed To Add Document To Collection '{collectionName}'. {e.Message}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        //  Purpose:        Get document by filter of collection 'collectionName'
        //  Input:          collectionName 
        //                  filter
        //                  value
        //                  document - [out] the document
        //                  result - [out] result
        //  Output:         true / false
        private bool TryGetDocument(string collectionName,
                                   BsonDocument filter,
                                   out List<string> document,
                                   out string result)
        {
            result = string.Empty;

            string method = MethodBase.GetCurrentMethod().Name;
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            if (null == collection)
            {
                result = "A Collection With The Name:[" + collectionName + "] Doesn't Exist.";

                Audit(result, method, LINE(), AuditSeverity.Warning);
                document = default;

                return false;
            }
            try
            {
                var queryResult = collection.Find(filter);
                //context = queryResult.ToList();
                document = new List<string>();

                return true;
            }

            catch (Exception e)
            {
                result = e.Message;

                Audit($"Failed To Retrieve Document From Collection [{collectionName}]. {e.Message}", method, LINE(), AuditSeverity.Error);
            }

            document = default;

            return false;
        }

        //  Purpose:        Document exists by filter in collection 'collectionName'
        //  Input:          collectionName 
        //                  filter
        //                  result - [out] result
        //  Output:         true / false
        private bool DocumentExists(string collectionName, BsonDocument filter, out string result)
        {
            result = string.Empty;

            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            if (null == collection)
            {
                result = "A collection with the name:[" + collectionName + "] doesn't exist.";

                Audit(result, method, LINE(), AuditSeverity.Warning);

                return false;
            }
            try
            {
                if (collection.Find(filter).FirstOrDefault() == null)
                {
                    result = "Document not found by filter";

                    return false;
                }

                return true;
            }

            catch (Exception e)
            {
                result = e.Message;

                Audit($"Failed to retrieve document from collection [{collectionName}]. {e.Message}", 
                      method, 
                      LINE(), 
                      AuditSeverity.Error);
            }

            return false;
        }

        //  Purpose:        Get all document by filter from collection 'collectionName'
        //  Input:          collectionName 
        //                  filter
        //                  documents - [out] list of document
        //                  result - [out] result
        //  Output:         true / false
        private bool TryGetAllDocuments(string collectionName, Dictionary<string, string> filter, out List<List<string>> documents, out string result)
        {
            bool matchFlag;

            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";

            var collection = _database.GetCollection<Dictionary<string, object>>(collectionName);

            result = string.Empty;

            if (collection == null)
            {
                result = "A collection with the name:[" + collectionName + "] doesn't exist.";

                Audit(result, method, LINE(), AuditSeverity.Warning);
                documents = default;

                return false;
            }

            documents = new List<List<string>>();

            try
            {
                var query = collection.AsQueryable<Dictionary<string, object>>();

                List<Dictionary<string, object>> dictionaries = query.ToList();

                for (int i = 0; i < dictionaries.Count; i++)
                {
                    Dictionary<string, object> document = dictionaries[i];

                    if (document != null)
                    {
                        if (filter == null)
                        {
                            matchFlag = true;
                        }
                        else
                        {
                            matchFlag = DocumentMatchesFilter(document, filter);
                        }

                        if (!matchFlag)
                        {
                            continue;
                        }

                        List<string> fields = new List<string>();

                        for (int j = 1; j < document.Count; j++)
                        {
                            fields.Add(document.ElementAt(j).Value.ToString());
                        }

                        documents.Add(fields);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                Audit($"Failed To Retrieve Documents From Collection In [{collectionName}] Collection. {e.Message}", 
                      method, 
                      LINE(), 
                      AuditSeverity.Error);
            }

            documents = default;

            return false;
        }

        //  Purpose:        Document matches filter criteria?
        //  Input:          document 
        //                  filter
        //  Output:         true / false
        private bool DocumentMatchesFilter(Dictionary<string, object> document, Dictionary<string, string> filter)
        {
            string method = "[" + MethodBase.GetCurrentMethod().Name + "]: ";
            string key;
            string value;

            int match = 1;

            try
            {
                if (filter != null)
                {
                    for (int i = 0; i < filter.Count; i++)
                    {
                        key = filter.ElementAt(i).Key;
                        value = filter.ElementAt(i).Value;

                        if (document.ContainsKey(key))
                        {
                            if (document.TryGetValue(key, out object documentValue))
                            {
                                if (value == (string)documentValue)
                                {
                                    match *= 1;
                                }
                                else
                                {
                                    match *= 0;
                                }
                            }
                        }
                    }
                }

                return (match > 0);
            }
            catch (Exception e)
            {
                Audit($"Failed To Match. {e.Message}", method, LINE(), AuditSeverity.Error);

                return false;
            }
        }

        #endregion

        #region Sql

        //  Purpose:        execute a SQL command and return the results
        //  Input:          sqlCommand 
        //                  [out] records
        //                  [out] result
        //  Output:         true / false
        public bool Query(string sqlCommand, out List<List<string>> records, out string result)
        {
            result = string.Empty;

            records = null;

            try
            {
                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        //  Purpose:        execute a SQL command
        //  Input:          sqlCommand 
        //                  [out] result
        //  Output:         true / false
        public bool NonQuery(string sqlCommand, out string result)
        {
            result = string.Empty;

            try
            {
                return true;
            }
            catch (Exception e)
            {
                result = e.Message;

                return false;
            }
        }

        #endregion

        #region Event Handlers

        //  Purpose:        throw 'message' to whom registered on the event ContextRepositoryDatabaseMessage
        //  Input:          message 
        //  Output:         none
        //private void OnContextRepositoryDatabaseMessage(string message)
        //{
        //    RepositoryDatabaseMessage?.Invoke(message, null);
        //}

        public void OnMessage(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            Message?.Invoke(message, method, module, line, auditSeverity);
        }

        #endregion

        private void Audit(string message, string method, string module, int line, AuditSeverity auditSeverity)
        {
            OnMessage(message, method, module, line, auditSeverity);
        }

        private void Audit(string message, string method, int line, AuditSeverity auditSeverity)
        {
            string module = "Mongo DB DAL";

            Audit(message, method, module, line, auditSeverity);
        }

        public static int LINE([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
    }
}
