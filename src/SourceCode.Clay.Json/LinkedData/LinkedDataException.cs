#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.Serialization;

namespace SourceCode.Clay.Json.LinkedData
{
    public class LinkedDataException : Exception
    {
        public LinkedDataErrorCode ErrorCode { get; }

        public LinkedDataException()
        {
        }

        public LinkedDataException(string message)
            : base(message)
        {
        }

        public LinkedDataException(LinkedDataErrorCode error)
            : base(GetMessage(error))
        {
        }

        public LinkedDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public LinkedDataException(LinkedDataErrorCode error, Exception innerException)
            : base(GetMessage(error), innerException)
        {
        }

        protected LinkedDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode = (LinkedDataErrorCode)info.GetInt32(nameof(ErrorCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), (int)ErrorCode);
        }

        private static string GetMessage(LinkedDataErrorCode error) => error.ToString();
    }
}
