using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NoDb.Web.Data;
using NoDb.Web.Models.Configuration;
using NoDb.Web.Models.Cues;

namespace NoDb.Web.Services
{
    public class CuesRepository : ICuesRepository
    {
        private readonly CuesMongoContext _context;

        public CuesRepository(IOptions<AppOptions> settings)
        {
            _context = new CuesMongoContext(settings);
        }

        public async Task<IEnumerable<CueMongo>> GetAllCues()
        {
            try
            {
                return await _context.Cues
                    .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        //
        public async Task<CueMongo> GetCue(string id)
        {
            try
            {
                var internalId = GetInternalId(id);
                return await _context.Cues
                    .Find(question => question.CueId == id
                                      || question.Id == internalId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<CueMongo>> GetCues(string question, int categoryNumber)
        {
            try
            {
                var query = _context.Cues.Find(q => q.Question.Contains(question) &&
                                                    q.Category.CategoryNumber == categoryNumber);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }

        public async Task AddCue(CueMongo item)
        {
            try
            {
                await _context.Cues.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveCue(string id)
        {
            try
            {
                var actionResult = await _context.Cues.DeleteOneAsync(Builders<CueMongo>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateCue(string id, string question, string answer, int sequence)
        {
            var filter = Builders<CueMongo>.Filter.Eq(s => s.CueId, id);
            var update = Builders<CueMongo>.Update
                .Set(s => s.Question, question)
                .Set(s => s.Answer, answer)
                .Set(s => s.Sequence, sequence)
                .CurrentDate(s => s.Updated);

            try
            {
                var actionResult = await _context.Cues.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                       && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateCue(string id, CueMongo item)
        {
            try
            {
                var actionResult
                    = await _context.Cues
                        .ReplaceOneAsync(n => n.CueId.Equals(id)
                            , item
                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                       && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // Demo function - full document update
        public async Task<bool> UpdateCueDocument(string id, string question, string answer, int sequence)
        {
            var item = await GetCue(id) ?? new CueMongo();
            item.Question = question;
            item.Answer = answer;
            item.Sequence = sequence;
            item.Updated = DateTime.Now;

            return await UpdateCue(id, item);
        }

        public async Task<bool> RemoveAllCues()
        {
            try
            {
                var actionResult = await _context.Cues.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<string> CreateIndex()
        {
            try
            {
                IndexKeysDefinition<CueMongo> keys = Builders<CueMongo>
                    .IndexKeys
                    .Ascending(item => item.CueId)
                    .Ascending(item => item.Question);

                return await _context.Cues
                    .Indexes.CreateOneAsync(new CreateIndexModel<CueMongo>(keys));
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}