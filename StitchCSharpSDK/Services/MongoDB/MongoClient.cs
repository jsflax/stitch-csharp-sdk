using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Stitch.Services.MongoDB
{
    /// <summary>
    /// MongoClient provides a simple wrapper around pipelines to enable 
    /// CRUD usage of a MongoDB service.
    /// </summary>
    public class MongoClient
    {
        private readonly StitchClient _stitchClient;
        private readonly string _service;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Stitch.Services.MongoDB.MongoClient"/> class.
        /// </summary>
        /// <param name="stitchClient">The client to execute with.</param>
        /// <param name="service">The name of the MongoDB service.</param>
        public MongoClient(StitchClient stitchClient, string service)
        {
            this._stitchClient = stitchClient;
            this._service = service;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>A reference to the database.</returns>
        /// <param name="name">The name of the database.</param>
        public Database GetDatabase(string name)
        {
            return new Database(this, name);
        }

        /// <summary>
        /// Database represents a reference to a MongoDB database accessed 
        /// through Stitch.</summary>
        public class Database
        {
            internal readonly MongoClient _client;
            internal readonly string _dbName;

            ///<param name="client"> The client to which this database is 
            ///referenced by.</param>
            /// <param name="dbName"> dbName The name of the database.</param>
            public Database(MongoClient client, string dbName)
            {
                this._client = client;
                this._dbName = dbName;
            }

            /// <summary>
            /// Gets a collection in this database.
            /// </summary>
            /// <returns>A reference to the collection.</returns>
            /// <param name="name">The name of the collection.</param>
            public Collection GetCollection(string name)
            {
                return new Collection(this, name);
            }
        }

        /// <summary>
        /// Collection represents a reference to a MongoDB collection 
        /// accessed through Stitch.
        /// </summary>
        public class Collection
        {
            private readonly Database _database;
            private readonly string _collName;

            public Collection(Database database, string collName)
            {
                this._database = database;
                this._collName = collName;
            }

            #region Pipeline Creation
            /// <summary>
            /// Makes a stage that executes a find on the collection.
            /// </summary>
            /// <returns>A stage representing this CRUD action.</returns>
            /// <param name="query">The query specifier.</param>
            /// <param name="projection">The projection document.</param>
            /// <param name="limit">The maximum amount of matching documents 
            /// to accept.</param>
            /// <param name="count">Whether or not to output a count of 
            /// documents matching the query.</param>
            public PipelineStage MakeFindStage(
                BsonDocument query,
                BsonDocument projection = null,
                int? limit = null,
                bool? count = null
            )
            {
                var args = new Dictionary<string, object>
                {
                    { Parameters.DATABASE, _database._dbName },
                    { Parameters.COLLECTION, _collName },
                    { Parameters.QUERY, query }
                };

                if (projection != null)
                {
                    args[Parameters.PROJECT] = projection;
                }
                if (limit != null)
                {
                    args[Parameters.LIMIT] = limit;
                }
                if (count != null)
                {
                    args[Parameters.COUNT] = count;
                }

                return new PipelineStage(
                        Stages.FIND,
                        _database._client._service,
                        args
                );
            }

            /// <summary>
            /// Makes a stage that executes an update on the collection.
            /// </summary>
            /// <returns>The update stage.</returns>
            /// <param name="query">The query specifier.</param>
            /// <param name="update">The update specifier.</param>
            /// <param name="upsert">Whether or not to upsert if the query
            /// matches no document</param>
            /// <param name="multi">Whether or not to update multiple 
            /// documents.</param>
            public PipelineStage MakeUpdateStage(
                BsonDocument query,
                BsonDocument update,
                bool upsert,
                bool multi
            )
            {
                var args = new Dictionary<string, object>
                {
                    { Parameters.DATABASE, _database._dbName },
                    { Parameters.COLLECTION, _collName },
                    { Parameters.QUERY, query },
                    { Parameters.UPDATE, update },
                    { Parameters.UPSERT, upsert },
                    { Parameters.MULTI, multi }
                };

                return new PipelineStage(
                        Stages.UPDATE,
                        _database._client._service,
                        args
                );
            }

            /// <summary>
            /// Makes a series of stages that execute an insert on the collection.
            /// </summary>
            /// <returns>The stages representing this CRUD action.</returns>
            /// <param name="documents">The set of documents to insert.</param>
            public List<PipelineStage> MakeInsertStage(
                params BsonDocument[] documents
            )
            {
                var literalArgs = new Dictionary<string, object>
                {
                    { PipelineStage.LiteralStage.PARAMETER_ITEMS, documents }
                };

                var insertArgs = new Dictionary<string, object>
                {
                    { Parameters.DATABASE, _database._dbName },
                    { Parameters.COLLECTION, _collName }
                };

                var pipelineStages = new List<PipelineStage>
                {
                    new PipelineStage(PipelineStage.LiteralStage.NAME,
                                      args: literalArgs),
                    new PipelineStage(Stages.INSERT,
                                      _database._client._service,
                                      insertArgs)
                };

                return pipelineStages;
            }

            /// <summary>
            /// Makes a stage that executes a delete on the collection.
            /// </summary>
            /// <returns>A stage representing this CRUD action.</returns>
            /// <param name="query">The query specifier.</param>
            /// <param name="singleDoc">Whether or not to delete only a single
            ///  matched document.</param>
            public PipelineStage MakeDeleteStage(BsonDocument query,
                                                 bool singleDoc)
            {
                var args = new Dictionary<string, object>
                {
                    { Parameters.DATABASE, _database._dbName },
                    { Parameters.COLLECTION, _collName },
                    { Parameters.QUERY, query },
                    { Parameters.SINGLE_DOCUMENT, singleDoc }
                };

                return new PipelineStage(Stages.DELETE,
                                         _database._client._service,
                                         args);
            }
			#endregion

			#region Queries
			/// <summary>
			/// Finds and projects documents matching a query.
			/// </summary>
			/// <returns>A task containing the matched and projected documents 
			/// that can be resolved upon completion of the request.</returns>
			/// <param name="query">The query specifier.</param>
			/// <param name="projection">The projection document..</param>
			/// <param name="limit">The maximum amount of matching documents 
            /// to accept.</param>
			/// <param name="count">Whether or not to include the count</param>
			public async Task<StitchResult<List<BsonDocument>>> Find(
                BsonDocument query,
                BsonDocument projection = null,
                int? limit = null,
                bool? count = null)
            {
                return await _database._client._stitchClient.ExecutePipeline(
                    this.MakeFindStage(query, projection, limit, count)
                );
            }

			/// <summary>
			/// Counts the number of documents matching a query up to 
			/// the specified limit.
			/// </summary>
			/// <returns>A task containing the number of matched documents that
            ///  can be resolved upon completion of the request.</returns>
			/// <param name="query">The query specifier.</param>
			/// <param name="limit">The maximum amount of matching documents 
            /// to accept.</param>
			public async Task<StitchResult<int>> Count(BsonDocument query,
                                                       int? limit)
            {
                var result = await _database._client._stitchClient.ExecutePipeline(
                    this.MakeFindStage(query, limit: limit, count: true)
                );

                var newResult = new StitchResult<int>();

                if (result.IsSuccessful)
                {
                    newResult.Value = result.Value[0].AsInt32;
                }
                else
                {
                    newResult.Error = result.Error;
                }

                return newResult;
            }

            public async Task<StitchResult<bool>> UpdateOne(BsonDocument query, 
                                                            BsonDocument update,
                                                            bool upsert)
            {
                var result = await this._database
                                       ._client
                                        ._stitchClient
                                        .ExecutePipeline(
                   this.MakeUpdateStage(query, update, upsert, false)
                );

                return new StitchResult<bool>(
                    result.IsSuccessful,
                    result.IsSuccessful,
                    result.Error
                );
            }

            public async Task<StitchResult<bool>> UpdateMany(BsonDocument query,
                                               BsonDocument update,
                                               bool upsert)
            {
                var result = await this._database
                                       ._client
                                       ._stitchClient
                                       .ExecutePipeline(
                    this.MakeUpdateStage(query, update, upsert, true)
                );


				return new StitchResult<bool>(
					result.IsSuccessful,
					result.IsSuccessful,
					result.Error
				);
            }

            public async Task<StitchResult<bool>> InsertOne(BsonDocument document)
            {
                var result = await this._database
                                       ._client
                                       ._stitchClient
                                       .ExecutePipeline(
                   this.MakeInsertStage(document).ToArray()
                );

				return new StitchResult<bool>(
					result.IsSuccessful,
					result.IsSuccessful,
					result.Error
				);
            }

            public async Task<StitchResult<bool>> InsertMany(List<BsonDocument> documents)
            {
                var result = await this._database
                                       ._client
                                       ._stitchClient
                                       .ExecutePipeline(
                   this.MakeInsertStage(documents.ToArray()).ToArray()
                );

				return new StitchResult<bool>(
					result.IsSuccessful,
					result.IsSuccessful,
					result.Error
				);
            }

            public async Task<StitchResult<bool>> DeleteOne(BsonDocument query)
            {
                var result = await this._database
                                       ._client
                                       ._stitchClient
                                       .ExecutePipeline(this.MakeDeleteStage(query, true));

                return new StitchResult<bool>(
					result.IsSuccessful,
					result.IsSuccessful,
					result.Error
				);                    
            }

			public async Task<StitchResult<bool>> DeleteMany(BsonDocument query)
			{
				var result = await this._database
									   ._client
									   ._stitchClient
									   .ExecutePipeline(this.MakeDeleteStage(query, false));

				return new StitchResult<bool>(
					result.IsSuccessful,
					result.IsSuccessful,
					result.Error
				);
			}
            #endregion
        }

        private static class Stages
        {
            internal const string FIND = "find";
            internal const string UPDATE = "update";
            internal const string INSERT = "insert";
            internal const string DELETE = "delete";
        }

        private static class Parameters
        {
            internal const string DATABASE = "database";
            internal const string COLLECTION = "collection";
            internal const string QUERY = "query";
            internal const string UPDATE = "update";
            internal const string UPSERT = "upsert";
            internal const string MULTI = "multi";
            internal const string PROJECT = "project";
            internal const string SINGLE_DOCUMENT = "singleDoc";
            internal const string LIMIT = "limit";
            internal const string COUNT = "count";
        }
    }
}
