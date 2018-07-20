using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Part
{
    internal static class Resources
    {
        internal const string WebService_InvalidGenerateScriptType = "Using the GenerateScriptTypes attribute is not supported for types in the following categories: primitive types; DateTime; generic types taking more than one parameter; types implementing IEnumerable or IDictionary; interfaces; Abstract classes; classes without a public default constructor.";
        internal const string WebService_MissingArg = "Invalid web service call,'{0}' missing value for parameter: '{1}'.";
        internal const string Session_TimeoutError = "会话已失效，请重新登录！";
        internal const string Session_UpgradeError = "产品已升级，会话已失效，请按F5刷新或重新登录后才能继续使用!";
        internal const string JSON_MaxJsonLengthExceeded = "结果记录数太大，请调整查询条件后重试";
        internal const string JSON_InvalidMaxJsonLength = "Value must be a positive integer.";
        internal const string JSON_InvalidRecursionLimit = "RecursionLimit must be a positive integer.";
        internal const string JSON_DictionaryTypeNotSupported = "Type '{0}' is not supported for serialization/deserialization of a dictionary, keys must be strings or objects.";
        internal const string JSON_DepthLimitExceeded = "RecursionLimit exceeded.";
        internal const string JSON_CircularReference = "A circular reference was detected while serializing an object of type '{0}'.";
        internal const string JSON_NoConstructor = "No parameterless constructor defined for type of '{0}'.";
        internal const string JSON_DeserializerTypeMismatch = "Cannot deserialize object graph into type of '{0}'.";
        internal const string JSON_ValueTypeCannotBeNull = "不能将null强制转为值类型 '{0}'.";
        internal const string JSON_CannotConvertObjectToType = "Cannot convert object of type '{0}' to type '{1}'";
        internal const string JSON_CannotCreateListType = "Cannot create instance of {0}.";
        internal const string JSON_ArrayTypeNotSupported = "Type '{0}' is not supported for deserialization of an array.";
        internal const string JSON_IllegalPrimitive = "Invalid JSON primitive: {0}.";
        internal const string JSON_InvalidArrayStart = "Invalid array passed in, '[' expected.";
        internal const string JSON_InvalidArrayExpectComma = "Invalid array passed in, ',' expected.";
        internal const string JSON_InvalidArrayExtraComma = "Invalid array passed in, extra trailing ','.";
        internal const string JSON_InvalidArrayEnd = "Invalid array passed in, ']' expected.";
        internal const string JSON_ExpectedOpenBrace = "Invalid object passed in, '{' expected.";
        internal const string JSON_InvalidMemberName = "Invalid object passed in, member name expected.";
        internal const string JSON_InvalidObject = "无效的JSON对象传入‘:’或‘}’不符合预期";
        internal const string JSON_UnterminatedString = "Unterminated string passed in.";
        internal const string JSON_BadEscape = "Unrecognized escape sequence.";
        internal const string JSON_StringNotQuoted = "Invalid string passed in, '\"' expected.";

        internal const string WebService_InvalidWebServiceCall = "Invalid web service call, expected path info of /js/<Method>.";
        internal const string WebService_InvalidVerbRequest = "An attempt was made to call the method '{0}' using a {1} request, which is not allowed.";
        internal const string WebService_Error = "There was an error processing the request.";
    }
}
