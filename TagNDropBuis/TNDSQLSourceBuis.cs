using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDSQLSourceBuis : ITNDSourceBuis {
        private ITNDApplicationBuis _ApplicationBuis;
        private TNDMetaSource _MetaSource;

        public ITNDSourceBuis Init(ITNDApplicationBuis applicationBuis, TNDMetaSource metaSource) {
            this._ApplicationBuis = applicationBuis;
            this._MetaSource = metaSource;
            return this;
        }

        public async Task<TNDEntityItem[]> SearchTerm(TNDMetaEntity metaEntity, string searchTerm, string emails, string emailDomains) {
            if (searchTerm == null) { searchTerm = string.Empty; }
            if (searchTerm.Length > 4000) { searchTerm = searchTerm.Substring(0, 4000); }
            string spName = $"SearchTerm{metaEntity.MetaEntityName}";
            //
            var result = new List<TNDEntityItem>();
            //
            System.Data.SqlClient.SqlConnectionStringBuilder csb = this.getConnectionString();
            using (var con = new System.Data.SqlClient.SqlConnection(csb.ConnectionString)) {
                var taskOpen = con.OpenAsync();
                try {
                    await taskOpen;
                } catch (Exception exception) {
                    throw;
                }
                using (var cmd = new System.Data.SqlClient.SqlCommand()) {
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = spName;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@term", System.Data.SqlDbType.NVarChar, -1) { Value = (searchTerm == null) ? DBNull.Value : (object)searchTerm });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@emails", System.Data.SqlDbType.NVarChar, -1) { Value = (emails == null) ? DBNull.Value : (object)emails });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@emailDomains", System.Data.SqlDbType.NVarChar, -1) { Value = (emailDomains == null) ? DBNull.Value : (object)emailDomains });
                    var taskExecuteReaderAsync = cmd.ExecuteReaderAsync();
                    System.Data.SqlClient.SqlDataReader sqlDataReader = null;
                    try {
                        sqlDataReader = await taskExecuteReaderAsync;
                    } catch (Exception exception) {
                        throw;
                    }
                    var taskReadFromDataReader = this.readFromDataReader(metaEntity, result, sqlDataReader);
                    try {
                        await taskReadFromDataReader;
                    } catch (Exception exception) {
                        throw;
                    }
                }
            }
            return result.ToArray();
        }

        public async Task<TNDEntityItem[]> Query(TNDMetaEntity metaEntity, string ids, bool returnParent, bool returnThis, bool returnChildren) {
            if (string.IsNullOrEmpty(ids)) { return new TNDEntityItem[0]; }
            string spName = $"Query{metaEntity.MetaEntityName}";
            //
            var result = new List<TNDEntityItem>();
            //
            System.Data.SqlClient.SqlConnectionStringBuilder csb = this.getConnectionString();
            using (var con = new System.Data.SqlClient.SqlConnection(csb.ConnectionString)) {
                var taskOpen = con.OpenAsync();
                try {
                    await taskOpen;
                } catch (Exception exception) {
                    throw;
                }
                using (var cmd = new System.Data.SqlClient.SqlCommand()) {
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = spName;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ids", System.Data.SqlDbType.NVarChar, -1) { Value = (ids == null) ? DBNull.Value : (object)ids });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@returnParent", System.Data.SqlDbType.Bit) { Value = returnParent });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@returnThis", System.Data.SqlDbType.Bit) { Value = returnThis });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@returnChildren", System.Data.SqlDbType.Bit) { Value = returnChildren });
                    var taskExecuteReaderAsync = cmd.ExecuteReaderAsync();
                    System.Data.SqlClient.SqlDataReader sqlDataReader = null;
                    try {
                        sqlDataReader = await taskExecuteReaderAsync;
                    } catch (Exception exception) {
                        throw;
                    }
                    var taskReadFromDataReader = this.readFromDataReader(metaEntity, result, sqlDataReader);
                    try {
                        await taskReadFromDataReader;
                    } catch (Exception exception) {
                        throw;
                    }
                }
            }
            return result.ToArray();
        }

        private System.Data.SqlClient.SqlConnectionStringBuilder getConnectionString() {
            string connectionString = this._MetaSource.Configuration;
            var csb = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            csb.AsynchronousProcessing = true;
            return csb;
        }

        private async Task readFromDataReader(TNDMetaEntity metaEntity, List<TNDEntityItem> result, System.Data.SqlClient.SqlDataReader sqlDataReader) {
            // loop over NextResult
            bool next = true;
            while (next) {
                var fieldCount = sqlDataReader.FieldCount;
                string[] names = new string[fieldCount];
                Type[] types = new Type[fieldCount];
                TNDPropertyCache[] caches = new TNDPropertyCache[fieldCount];
                object[] values = new object[fieldCount];
                int idxMetaEntityName = -1;
                for (int idx = 0; idx < fieldCount; idx++) {
                    names[idx] = sqlDataReader.GetName(idx);
                    types[idx] = sqlDataReader.GetFieldType(idx);
                    caches[idx] = new TNDPropertyCache();
                    if (string.Equals(names[idx], "MetaEntityName", StringComparison.Ordinal)) {
                        idxMetaEntityName = idx;
                    }
                }
                // loop over Read
                bool read = true;
                while (read) {
                    var taskReadAsync = sqlDataReader.ReadAsync();
                    try {
                        read = await taskReadAsync;
                    } catch (Exception exception) {
                        throw;
                    }
                    if (read) {
                        sqlDataReader.GetValues(values);
                        var item = new TNDEntityItem();
                        for (int idx = 0; idx < fieldCount; idx++) {
                            if (idx == idxMetaEntityName) {
                                item.MetaEntityName = values[idx] as string;
                            } else {
                                var property = new TNDProperty(names[idx], types[idx], values[idx]);
                                if (caches[idx] != null) {
                                    property = caches[idx].GetOrCreate(property);
                                }
                                item.Property.Add(property);
                            }
                        }
                        if (item.MetaEntityName != null) {
                            item.MetaEntity = this._ApplicationBuis.ApplicationModel.Configuration.FindMetaEntity(item.MetaEntityName);
                        }
                        if (item.MetaEntity == null) {
                            item.MetaEntity = metaEntity;
                            item.MetaEntityName = metaEntity.MetaEntityName;
                        }
                        result.Add(item);
                    }
                }
                var taskNextResult = sqlDataReader.NextResultAsync();
                try {
                    next = await taskNextResult;
                } catch (Exception exception) {
                    throw;
                }
            }
        }
    }
}
