using System.Collections.Generic;
using Alpha.Maintenance.Core.Model;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.DbProfiler
{
    public class LogService
    {
        private readonly IDynamicRepository _dynamicRepository;

        private int _page;

        private int _size;

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
            _page = 0;
            _size = 100;
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
        public LogService WithPagination(int page, int size)
        {
            this._page = page;
            this._size = size;

            return this;
        }

        public IEnumerable<DbProfilerViewModel> GetLogs(ref bool hasError, ref string errorMsg)
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

            hasError = criteria.HasError;
            errorMsg = criteria.ErrorMsg;

            return list;
        }
    }
}
