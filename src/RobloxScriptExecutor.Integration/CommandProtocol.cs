using System;

namespace RobloxScriptExecutor.Integration
{
    public record ExecutionRequest(string ScriptContent, string ScriptName, DateTime Timestamp);

    public record BridgeResponse(bool HasNewScript, string? ScriptContent, string? ScriptName);
}
