using System.Collections.Generic;
using Alpha.Maintenance.Core.Model;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.DbProfiler
{
    public class LogService
    {
        private readonly IDynamicRepository _dynamicRepository;

        private string _site;

        private int _traceTimer;

        private string _textDataLike;

        private string _textDataLike2;

        private string _textDataLike3;

        private bool _traceStartEvents;

        private bool _mode;

        private string _server;

        public LogService()
        {
            _dynamicRepository = new DynamicRepository();
        }

        public LogService With(DBProfilerModel model)
        {
            _site = model.Site;
            _traceTimer = model.TraceTimer;
            _textDataLike = model.TextDataLike;
            _textDataLike2 = model.TextDataLike2;
            _textDataLike3 = model.TextDataLike3;
            _traceStartEvents = model.TraceStartEvents;
            _mode = false;
            _server = model.Server;

            return this;
        }

        public (IEnumerable<DbProfilerViewModel>, bool, string) GetLogs()
        {
            var criteria = new DBProfilerModel
            {
                Site = this._site,
                Mode = this._mode,
                TextDataLike = this._textDataLike,
                TextDataLike2 = this._textDataLike2,
                TextDataLike3 = this._textDataLike3,
                TraceStartEvents = this._traceStartEvents,
                TraceTimer = this._traceTimer,
                Server = this._server,
                HasError = false,
                ErrorMsg = string.Empty
            };

            var list = _dynamicRepository.Fetch<DbProfilerViewModel>(criteria, criteria.Server);

            return (list, criteria.HasError, criteria.ErrorMsg);
        }
    }
}
