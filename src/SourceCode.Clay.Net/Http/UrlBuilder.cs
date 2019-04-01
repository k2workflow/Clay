using System;
using System.Text;

namespace SourceCode.Clay.Net.Http
{
    internal struct UrlBuilder
    {
        private const byte State_Path = 0;
        private const byte State_ParameterName = 1;
        private const byte State_ParameterValueStart = 2;
        private const byte State_ParameterValue = 3;

        private readonly StringBuilder _builder;
        private byte _state;

        public UrlBuilder(int capacity) => (_builder, _state) = (new StringBuilder(capacity), State_Path);

        public UrlBuilder Append(string value)
        {
            if (value == null) return this;

            if (_state == State_ParameterValueStart)
            {
                _builder.Append("=");
                _state = State_ParameterValue;
            }
            _builder.Append(value);
            return this;
        }

        public UrlBuilder StartParameter()
        {
            if (_state == State_Path) _builder.Append("?");
            else _builder.Append("&");
            _state = State_ParameterName;
            return this;
        }

        public UrlBuilder StartValue()
        {
            _state = State_ParameterValueStart;
            return this;
        }

        public override string ToString() => _builder.ToString();
    }
}
